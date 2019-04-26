using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆発するEnemyクラス（モチーフ：水瓶座）
/// </summary>
public class ExplosionEnemy : Enemy
{
    Transform parent;

    [SerializeField, Header("爆発までの間隔")]
    float interval = 2;
    float elapedTime = 0;//経過時間

    [SerializeField]
    float maxDistande = 2;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rotation = Quaternion.identity;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        state = State.NORMAL;

        maxSpeed = power / 10f;

        parent = transform.parent;
        changeDirection = GetComponent<ChangeDirection>();
        changeDirection.MaxDistance = maxDistande;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Direction();
        SetTarget();
        Death();
    }

    //public override void Move()
    //{
    //    //今のスピードを計算
    //    float nowSpeed = Mathf.Abs(rigid.velocity.x);

    //    //最大の移動スピードを超えていないとき
    //    if (nowSpeed < maxSpeed)
    //    {
    //        switch (state)
    //        {
    //            case State.NORMAL:
    //                rigid.AddForce(transform.forward * power, ForceMode.Acceleration);
    //                break;

    //            case State.CHASE:
    //                distance.x = target.position.x - transform.position.x;
    //                if (distance.x < 0)
    //                {
    //                    direction_Left = true;
    //                }
    //                else if (distance.x > 0)
    //                {
    //                    direction_Left = false;
    //                }
    //                rigid.AddForce(transform.forward * power, ForceMode.Acceleration);
    //                break;
    //        }
    //    }
    //}

    public override void Explosion()
    {
        elapedTime += Time.deltaTime;
        if (elapedTime >= interval)
        {
            Instantiate(downParticle, transform.position, Quaternion.identity, parent);
            EnemyManager.DefeatedCount++;
            Debug.Log("倒した数：" + EnemyManager.DefeatedCount);
            Destroy(gameObject);
        }
    }

    public override void Damage()
    {
        hp--;
    }

    public override void SetTarget()
    {
        if (!isChase) return;

        if (target.position.x - transform.position.x <= 5f
            && target.position.x - transform.position.x >= -5f)
        {
            state = State.CHASE;
        }
        else
        {
            state = State.NORMAL;
        }
    }

    public override void Death()
    {
        if (hp <= 0)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            Defeat = true;
            Explosion();
        }
    }
}
