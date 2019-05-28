using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 牡牛座のボス
/// </summary>
public class Taurus : BossEnemy
{
    [SerializeField, Header("移動力")]
    float power = 20;

    [SerializeField, Header("突進までの待機時間")]
    float interval = 5;
    float intervalElapsedTime;//待機中の経過時間

    int maxHp;
    [SerializeField, Header("巨大化サイズ")]
    float hugingScale = 1.5f;

    //全ての状態
    public enum Mode
    {
        WAIT,
        RUSH,
        STAN
    }
    [HideInInspector]
    public Mode mode;//現在の状態

    Vector3 targetPosition;//突進開始時のプレイヤーの位置

    bool isChangeNow = false;//方向転換中か

    [SerializeField]
    GameObject sandParticle;
    float instanteTime;

    Vector3 instantePosition;//パーティクルを生成する位置
    Vector3 particleRight = new Vector3(0, 90);//右側の生成方向
    Vector3 particleLeft = new Vector3(0, -90);//左側の生成方向

    Vector3 startScale;//スケールの初期値
    bool isHuging = false;//巨大化中か

    bool isTurn = false;
    int rushTurn = 0;

    float speed = 1;

    bool isPlaySe = false;
    Dictionary<int, int> SeInfo;//鳴らすSEのデータ用のディクショナリ
    Dictionary<int, float> ProbabilityDictionary;

    bool rushBeforePlay = false;

    AudioSource audioSource;

    [SerializeField, Header("ステージの左端")]
    Vector3 leftRangeVector = Vector3.zero;
    [SerializeField, Header("ステージの右端")]
    Vector3 rightRangeVector = Vector3.zero;

    Camera main;

    [SerializeField]
    GameObject hitParticle;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rotation = Quaternion.identity;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        mode = Mode.WAIT;//待機状態に設定

        OnRight = true;

        //全ての経過時間を0
        stanElapsedTime = 0;
        invincibleElapsedTime = 0;
        instanteTime = 0;

        anim = GetComponent<Animator>();

        startScale = transform.localScale;

        maxHp = hp;

        audioSource = GetComponent<AudioSource>();

        main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.isWishMode)
        {
            Stop();
            return;
        }

        Death();

        if (IsDead)
        {
            //死亡してから、アニメーションが終わるまでのおおよその時間経過でパーティクル生成、
            //かつ、return以下の処理を行わない
            deadElapsedTime += Time.deltaTime;
            if (deadElapsedTime >= 2)
            {
                Instantiate(downParticle, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            return;
        }

        if (hp <= maxHp / 2) speed = 1.5f;

        switch (mode)//状態に応じた処理を実行
        {
            case Mode.WAIT:
                Stop();
                StartCoroutine(DirectionCoroutine());
                RushPrepare();
                SetOnRight();
                anim.speed = 1 * speed;
                break;

            case Mode.RUSH:
                if(!isTurn)RushAttack();
                else TurnRush();
                break;

            case Mode.STAN:
                Stop();
                NowStan();
                anim.speed = 1;
                break;

            default:
                break;
        }

        HugingScale();
    }

    /// <summary>
    /// プレイヤーから見て、どちらにいるか
    /// </summary>
    void SetOnRight()
    {
        //ステージ右端にいるか、ステージ左端からプレイヤーが間にいれば
        if ((transform.position.x > target.position.x && target.position.x > leftRangeVector.x)
            || rightRangeVector.x - 1 <= transform.position.x)
            OnRight = true;//右

        //ステージ左端にいるか、ステージ右端からプレイヤーが間にいれば
        if ((transform.position.x < target.position.x && target.position.x < rightRangeVector.x) 
            || leftRangeVector.x + 1 >= transform.position.x)
            OnRight = false;//左
    }
    
    /// <summary>
    /// 砂埃のパーティクル生成
    /// </summary>
    void GenerateSandParticle()
    {
        GameObject particle;

        //向きに応じて、砂埃の方向を変更
        if (OnRight)
            particle = Instantiate(sandParticle, instantePosition, Quaternion.Euler(particleRight));
        else
            particle = Instantiate(sandParticle, instantePosition, Quaternion.Euler(particleLeft));

        particle.transform.localScale = transform.localScale / 150f;
    }

    /// <summary>
    /// 突進準備
    /// </summary>
    void RushPrepare()
    {
        if (isHuging) return;

        intervalElapsedTime += Time.deltaTime * speed;
        HugingScale();
        instanteTime += Time.deltaTime * speed;
        if (instanteTime >= 1)
        {
            GenerateSandParticle();
            instanteTime = 0;
        }
        
        if (intervalElapsedTime >= interval - 1)
        {
            if (!rushBeforePlay)
            {
                RandomPlaySE();
                rushBeforePlay = true;
            }
        }
        if(intervalElapsedTime >= interval)
        {
            if (isChangeNow) return;

            //突進に必要な情報を得る
            targetPosition = target.position;//突進開始時のプレイヤーの位置に向かう
            intervalElapsedTime = 0;
            rushBeforePlay = false;
            mode = Mode.RUSH;
        }
    }

    /// <summary>
    /// サイズ変化（巨大化）
    /// </summary>
    void HugingScale()
    {
        //体力が半分を切ったら巨大化
        if (hp > maxHp / 2) return;

        //巨大化
        if (transform.localScale.x <= startScale.x * hugingScale)
        {
            transform.localScale += new Vector3(10, 10, 10) * Time.deltaTime;
            isHuging = true;
        }
        else
        {
            isHuging = false;
        }
    }

    /// <summary>
    /// X軸の移動以外を固定
    /// </summary>
    void MoveFreeze()
    {
        rigid.constraints = RigidbodyConstraints.FreezeRotation 
            | RigidbodyConstraints.FreezePositionY 
            | RigidbodyConstraints.FreezePositionZ;
    }

    /// <summary>
    /// 突進攻撃
    /// </summary>
    void RushAttack()
    {
        if (isHuging)
        {
            anim.speed = 0;
            Stop();
            return;
        }

        MoveFreeze();

        rigid.AddForce(transform.forward * power * speed, ForceMode.Acceleration);

        if (!isPlaySe)
        {
            audioSource.PlayOneShot(audioSource.clip);
            isPlaySe = true;
        }

        if (Mathf.Abs(rigid.velocity.x) <= 10)
        {
            if (OnRight) rigid.velocity = new Vector3(-10, 0);
            else rigid.velocity = new Vector3(10, 0);
        }

        GenerateSandParticle();
        anim.speed = 3 * speed;
    }

    void TurnRush()
    {
        Stop();
        StartCoroutine(DirectionCoroutine());
        anim.speed = 1 * speed;
    }

    void NowStan()
    {
        stanElapsedTime += Time.deltaTime;
        if(stanElapsedTime >= stanTime)
        {
            anim.SetTrigger("stand");
            stanElapsedTime = 0;
            mode = Mode.WAIT;
        }
    }
    
    public override void OnCollisionEnter(Collision other)
    {
        if (mode != Mode.STAN)
        {
            //Playerに当たったらPlayerにダメージ
            if (other.gameObject.tag.Contains("Player"))
            {
                //その場で待機状態に移行
                PlayerManager.PlayerDamage(10);

                if (hp <= maxHp / 2 && mode == Mode.RUSH)
                {
                    BlowUp();
                }
                else
                {
                    Blow();
                    if (mode == Mode.RUSH)
                    {
                        mode = Mode.WAIT;
                    }
                }
            }
        }

        //突進中に壁に当たったらスタン状態に移行
        if (other.gameObject.name.Contains("Wall") && mode == Mode.RUSH)
        {
            if (hp <= maxHp / 2)
            {
                rushTurn++;
                OnRight = !OnRight;
                audioSource.Stop();
                isPlaySe = false;
                if (rushTurn >= 3)
                {
                    SoundManager.PlaySE(15);
                    rushTurn = 0;
                    anim.SetTrigger("down");
                    main.GetComponent<ShakeCamera>().Shake();
                    mode = Mode.STAN;
                    return;
                }
                isTurn = true;
            }
            else
            {
                SoundManager.PlaySE(15);
                audioSource.Stop();
                isPlaySe = false;
                anim.SetTrigger("down");
                main.GetComponent<ShakeCamera>().Shake();
                mode = Mode.STAN;
            }
        }

        if (other.gameObject.tag == "Star" && !isHuging)
        {
            Damage(1);

            Instantiate(hitParticle, other.contacts[0].point, Quaternion.identity);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Star"))
        {
            isHit = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Stage")
        {
            instantePosition = collision.contacts[0].point;
        }

        if(collision.gameObject.tag == "Player" && mode != Mode.STAN)
        {
            Blow();
        }
    }

    /// <summary>
    /// 時間をかけて回転させる
    /// </summary>
    /// <returns></returns>
    IEnumerator DirectionCoroutine()
    {
        float rate = 0;

        while (true)
        {
            rate += Time.deltaTime * 3 * speed;
            if (OnRight)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(forward), rate);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-forward), rate);
            }
            break;
        }

        //Playerの方向に向き終わるまでは方向転換中に設定
        if (GetAngle())
        {
            isChangeNow = false;

            if (mode == Mode.RUSH) isTurn = false;
        }
        else
        {
            isChangeNow = true;
        }

        yield return null;
    }

    bool GetAngle()
    {
        bool isForward;

        if (transform.rotation == Quaternion.Euler(new Vector3(0, -90, 0))
            || transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0))
            || transform.rotation == Quaternion.Euler(new Vector3(0, 270, 0))
            || transform.rotation == Quaternion.Euler(new Vector3(0, -270, 0)))
        {
            isForward = true;
        }
        else
        {
            isForward = false;
        }
        return isForward;
    }

    /// <summary>
    /// プレイヤーに当たったときに吹き飛ばす
    /// </summary>
    void Blow()
    {
        if (OnRight) target.GetComponent<PlayerController>().Damage(new Vector3(-5, 3));
        else target.GetComponent<PlayerController>().Damage(new Vector3(5, 3));
    }
    
    /// <summary>
    /// 打ち上げ
    /// </summary>
    void BlowUp()
    {
        if (OnRight) target.GetComponent<PlayerController>().Damage(new Vector3(-5, 15));
        else target.GetComponent<PlayerController>().Damage(new Vector3(5, 15));
    }

    /// <summary>
    /// ランダムでSEを再生
    /// </summary>
    void RandomPlaySE()
    {
        InitializeDictionary();
        int seID = Choose();

        SoundManager.PlaySE(SeInfo[seID]);
    }

    /// <summary>
    /// ディクショナリ初期化
    /// </summary>
    void InitializeDictionary()
    {
        SeInfo = new Dictionary<int, int>();
        SeInfo.Add(0, 30);
        SeInfo.Add(1, 31);
        SeInfo.Add(2, 32);
        SeInfo.Add(3, 33);
        SeInfo.Add(4, 34);

        ProbabilityDictionary = new Dictionary<int, float>();
        ProbabilityDictionary.Add(0, 20.0f);
        ProbabilityDictionary.Add(1, 20.0f);
        ProbabilityDictionary.Add(2, 20.0f);
        ProbabilityDictionary.Add(3, 20.0f);
        ProbabilityDictionary.Add(4, 20.0f);
    }

    int Choose()
    {
        float total = 0;

        foreach(KeyValuePair<int,float> elem in ProbabilityDictionary)
        {
            total += elem.Value;
        }

        float randomPoint = Random.value * total;

        foreach(KeyValuePair<int, float> elem in ProbabilityDictionary)
        {
            if(randomPoint<elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint -= elem.Value;
            }
        }

        return 1;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = Color.red;

        Gizmos.DrawCube(leftRangeVector, new Vector3(1,1,1));
        Gizmos.DrawCube(rightRangeVector, new Vector3(1, 1, 1));     
    }


}
