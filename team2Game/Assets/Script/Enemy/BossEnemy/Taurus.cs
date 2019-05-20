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

    [SerializeField]
    Vector3 originBlowVector;

    bool isTurn = false;
    int rushTurn = 0;

    float speed = 1;

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
    }

    // Update is called once per frame
    void Update()
    {
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
                anim.speed = 0;
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
        if (transform.position.x > target.position.x) OnRight = true;//右
        if (transform.position.x < target.position.x) OnRight = false;//左
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
        intervalElapsedTime += Time.deltaTime * speed;
        HugingScale();
        instanteTime += Time.deltaTime * speed;
        if (instanteTime >= 1)
        {
            GenerateSandParticle();
            instanteTime = 0;
        }

        //待機中の経過時間が規定時間まで過ぎたら突進状態へ移行
        if (intervalElapsedTime < interval) return;
        else
        {
            if (isChangeNow || isHuging) return;

            //突進に必要な情報を得る
            targetPosition = target.position;//突進開始時のプレイヤーの位置に向かう
            intervalElapsedTime = 0;
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
        MoveFreeze();

        rigid.AddForce(transform.forward * power * speed, ForceMode.Acceleration);
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

    /// <summary>
    /// スタン中の処理
    /// </summary>
    void NowStan()
    {
        stanElapsedTime += Time.deltaTime * speed;
        if (stanElapsedTime >= stanTime)
        {
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
                Blow();

                if (hp <= maxHp / 2 && mode == Mode.RUSH)
                {
                    BlowUp();
                }
                else
                {
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
                if (rushTurn >= 3)
                {
                    mode = Mode.STAN;
                    rushTurn = 0;
                    return;
                }
                isTurn = true;
            }
            else
            {
                mode = Mode.STAN;
            }
        }

        if (other.gameObject.tag == "Star" && !isHuging)
        {
            Damage(1);
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
        var blowVector = Vector3.zero;

        if (OnRight) blowVector = originBlowVector;
        else blowVector = new Vector3(-originBlowVector.x, originBlowVector.y);

        target.GetComponent<Rigidbody>().AddForce(blowVector, ForceMode.Impulse);
    }
    
    /// <summary>
    /// 打ち上げ
    /// </summary>
    void BlowUp()
    {
        var blowVector = Vector3.zero;

        if (OnRight) blowVector = new Vector3(-25, 20);
        else blowVector = new Vector3(25, 20);

        target.GetComponent<Rigidbody>().AddForce(blowVector, ForceMode.Impulse);
    }
}
