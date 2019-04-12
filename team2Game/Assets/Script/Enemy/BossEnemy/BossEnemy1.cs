using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode
{
    NORMAL,
    STAN,
    INVINCIBLE
}

public class BossEnemy1 : Enemy
{
    [SerializeField, Header("待機時間")]
    float interval = 5f;

    [SerializeField, Header("スタンの持続時間")]
    float stanTime = 3;
    float stanElapsedTime;//スタン中の経過時間

    [HideInInspector]
    public Mode mode;

    [HideInInspector]
    public bool isHit = true;//プレイヤーの攻撃が当たるかどうか
    [SerializeField, Header("無敵時間")]
    float invincibleTime = 3;
    float invincibleElapsedTime;//無敵時間の経過時間
    bool rushing = false;//突進中かどうか

    Vector3 targetPosition;//突進開始時のプレイヤーの位置
    Vector3 startPosition;//突進開始地点

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rotation = Quaternion.identity;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        mode = Mode.NORMAL;

        stanElapsedTime = 0;
        invincibleElapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case Mode.NORMAL:
                if (!rushing)
                {
                    Direction();
                }
                RushAttack();
                break;

            case Mode.STAN:
                NowStan();
                break;

            case Mode.INVINCIBLE:
                NowInvincible();
                break;

            default:
                break;
        }

        Death();
        Debug.Log(mode);
    }

    public override void Direction()
    {
        if(transform.position.x > target.position.x)
        {
            rotation = Quaternion.Euler(forward);
        }
        if (transform.position.x < target.position.x)
        {
            rotation = Quaternion.Euler(-forward);
        }
        transform.rotation = rotation;
    }

    /// <summary>
    /// 突進攻撃
    /// </summary>
    void RushAttack()
    {
        if (!rushing)
        {
            interval -= Time.deltaTime;
            if (interval > 0) return;
            else
            {
                startPosition = transform.position;
                targetPosition = target.position;
                interval = 5;
                rushing = true;
            }
        }
        else
        {
            rigid.AddForce(targetPosition - startPosition, ForceMode.Acceleration);
            //rigid.velocity /= 2f;
        }
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
            mode = Mode.NORMAL;
        }
    }

    /// <summary>
    /// 無敵時間の処理
    /// </summary>
    void NowInvincible()
    {
        invincibleElapsedTime += Time.deltaTime;

        if(invincibleElapsedTime >= invincibleTime)
        {
            invincibleElapsedTime = 0;
            mode = Mode.NORMAL;
            isHit = true;
        }
    }

    public void Stop()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    public override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            rushing = false;
            Stop();
            PlayerManager.PlayerDamage(10);
        }

        if (other.gameObject.name.Contains("Wall"))
        {
            rushing = false;
            Stop();
            mode = Mode.STAN;
        }
    }
}
