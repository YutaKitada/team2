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

    SkinnedMeshRenderer skinnedMeshRenderer;

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

        skinnedMeshRenderer = transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Death();

        if (IsDead)
        {
            skinnedMeshRenderer.enabled = true;
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
                anim.speed = 1;
                break;

            case Mode.RUSH:
                RushAttack();
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

        if (transform.position.x > target.position.x) onRight = true;
        if (transform.position.x < target.position.x) onRight = false;
    }

    /// <summary>
    /// 突進準備
    /// </summary>
    void RushPrepare()
    {
        intervalElapsedTime += Time.deltaTime;

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
            mode = Mode.RUSH;
        }
    }

    /// <summary>
    /// 突進攻撃
    /// </summary>
    void RushAttack()
    {
        rigid.AddForce(transform.forward * power, ForceMode.Acceleration);
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
