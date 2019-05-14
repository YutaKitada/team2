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

    bool isChange = false;//方向転換中か

    bool onRight = true;//プレイヤーより右側にいるか

    [SerializeField]
    GameObject sandParticle;
    float instanteTime;

    Vector3 instantePosition;
    Vector3 particleRight = new Vector3(0, 90);
    Vector3 particleLeft = new Vector3(0, -90);

    Vector3 startScale;
    int hitCount;
    bool isHuging = true;

    //bool isFeint = false;//フェイントをかけるか
    //Dictionary<int, bool> FeintInfo;//フェイントするかのディクショナリ
    //Dictionary<int, float> FeintDictionary;//上記のための確率用のディクショナリ

    //bool isChosen = false;//フェイントのboolが決められたか
    //float stopTime;//フェイント時の止まるまでの時間

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rotation = Quaternion.identity;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        mode = Mode.WAIT;//待機状態に設定

        //全ての経過時間を0
        stanElapsedTime = 0;
        invincibleElapsedTime = 0;

        instanteTime = 0;

        anim = GetComponent<Animator>();

        startScale = transform.localScale;
        hitCount = 0;

        //InitializeDictionary();
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

        switch (mode)//状態に応じた処理を実行
        {
            case Mode.WAIT:
                Stop();
                StartCoroutine(DirectionCoroutine());
                RushPrepare();
                ContractionScale();
                if (transform.position.x > target.position.x) onRight = true;
                if (transform.position.x < target.position.x) onRight = false;
                anim.speed = 1;
                break;

            case Mode.RUSH:
                RushAttack();
                ContractionScale();
                anim.speed = 3;
                break;

            case Mode.STAN:
                Stop();
                NowStan();
                anim.speed = 0;
                break;

            default:
                break;
        }
    }

    ///// <summary>
    ///// フェイント取得
    ///// </summary>
    //void GetFeint()
    //{
    //    if (isChosen) return;

    //    int Id = Choose();

    //    if (Id != 0)
    //    {
    //        isFeint = false;
    //    }
    //    else
    //    {
    //        isFeint = true;
    //    }

    //    isChosen = true;
    //}

    ///// <summary>
    ///// ディクショナリの初期化
    ///// </summary>
    //void InitializeDictionary()
    //{
    //    FeintInfo = new Dictionary<int, bool>();
    //    FeintInfo.Add(0, true);
    //    FeintInfo.Add(1, false);

    //    FeintDictionary = new Dictionary<int, float>();
    //    FeintDictionary.Add(0, 50f);
    //    FeintDictionary.Add(1, 50f);
    //}

    ///// <summary>
    ///// 確率を決める処理
    ///// </summary>
    ///// <returns></returns>
    //int Choose()
    //{
    //    float total = 0;

    //    foreach(KeyValuePair<int, float> elem in FeintDictionary)
    //    {
    //        total += elem.Value;
    //    }

    //    float randomPoint = Random.value * total;

    //    foreach(KeyValuePair<int, float> elem in FeintDictionary)
    //    {
    //        if (randomPoint < elem.Value)
    //        {
    //            return elem.Key;
    //        }
    //        else
    //        {
    //            randomPoint -= elem.Value;
    //        }
    //    }

    //    return 1;
    //}

    /// <summary>
    /// 突進準備
    /// </summary>
    void RushPrepare()
    {
        intervalElapsedTime += Time.deltaTime;
        HugingScale();
        instanteTime += Time.deltaTime;
        if (instanteTime >= 1)
        {
            if (onRight)
            {
                Instantiate(sandParticle, instantePosition, Quaternion.Euler(particleRight));
            }
            else
            {
                Instantiate(sandParticle, instantePosition, Quaternion.Euler(particleLeft));
            }
            instanteTime = 0;
        }

        //待機中の経過時間が規定時間まで過ぎたら突進状態へ移行
        if (intervalElapsedTime < interval) return;
        else
        {
            if (isChange) return;

            //突進に必要な情報を得る
            targetPosition = target.position;//突進開始時のプレイヤーの位置に向かう
            intervalElapsedTime = 0;
            //GetFeint();
            mode = Mode.RUSH;
        }
    }


    void HitCount()
    {
        if (!isHit) return;

        hitCount++;
        isHit = false;

        if (hitCount >= 3)
        {
            hitCount = 0;
            isHuging = false;
            mode = Mode.STAN;
        }
    }

    /// <summary>
    /// サイズ変化（巨大化）
    /// </summary>
    void HugingScale()
    {
        //巨大化
        if (transform.localScale.x <= startScale.x * 2 && isHuging)
        {
            transform.localScale += new Vector3(10, 10, 10) * Time.deltaTime * 2;
        }
    }

    /// <summary>
    /// サイズ変化（収縮）
    /// </summary>
    void ContractionScale()
    {
        //収縮
        if (transform.localScale.x >= startScale.x && !isHuging)
        {
            transform.localScale -= new Vector3(10, 10, 10) * Time.deltaTime * 3;
        }
    }

    /// <summary>
    /// 突進攻撃
    /// </summary>
    void RushAttack()
    {
        rigid.AddForce(transform.forward * power, ForceMode.Acceleration);
        ////フェイントするとき
        //if (isFeint)
        //{
        //    stopTime += Time.deltaTime;
        //    if (stopTime >= 1)
        //    {
        //        Stop();
        //        stopTime = 0;
        //        mode = Mode.WAIT;
        //    }
        //}

        //向きに応じて、砂埃の方向を変更
        if (onRight)
            Instantiate(sandParticle, instantePosition, Quaternion.Euler(particleRight));
        else
            Instantiate(sandParticle, instantePosition, Quaternion.Euler(particleLeft));

        //isChosen = false;
    }

    /// <summary>
    /// スタン中の処理
    /// </summary>
    void NowStan()
    {
        stanElapsedTime += Time.deltaTime;
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

                if (mode == Mode.RUSH)
                {
                    mode = Mode.WAIT;
                }
            }
        }

        //突進中に壁に当たったらスタン状態に移行
        if (other.gameObject.name.Contains("Wall") && mode == Mode.RUSH)
        {
            mode = Mode.STAN;
        }

        if (mode == Mode.WAIT && other.gameObject.tag.Contains("Star"))
        {
            HitCount();
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
            rate += Time.deltaTime * 3;
            if (onRight)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(forward), rate);
            }
            if (!onRight)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-forward), rate);
            }
            break;
        }

        //Playerの方向に向き終わるまでは方向転換中に設定
        if (transform.rotation == Quaternion.Euler(forward) ||
            transform.rotation == Quaternion.Euler(-forward))
        {
            isChange = false;
        }
        else
        {
            isChange = true;
        }

        yield return null;
    }
}
