using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 横移動のEnemyクラス（モチーフ：蟹座）
/// </summary>
public class Cancer : Enemy
{
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
        changeDirection = GetComponent<ChangeDirection>();
        changeDirection.MaxDistance = maxDistande;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.isWishMode || WishManager.wishProductionFlag) return;

        Move();
        SetTarget();
        Death();
    }

    public override void Move()
    {
        //今のスピードを計算
        float nowSpeed = Mathf.Abs(rigid.velocity.x);

        //最大の移動スピードを超えていないとき
        if (nowSpeed < maxSpeed)
        {
            switch (state)
            {
                case State.NORMAL:
                    if (direction_Left)
                    {
                        rigid.AddForce(-transform.right * power, ForceMode.Acceleration);
                    }
                    else
                    {
                        rigid.AddForce(transform.right * power, ForceMode.Acceleration);
                    }

                    NowChase = false;
                    break;

                case State.CHASE:
                    distance.x = target.position.x - transform.position.x;
                    if (distance.x < 0)
                    {
                        direction_Left = true;
                        rigid.AddForce(-transform.right * power, ForceMode.Acceleration);
                    }
                    else if (distance.x > 0)
                    {
                        direction_Left = false;
                        rigid.AddForce(transform.right * power, ForceMode.Acceleration);
                    }

                    NowChase = true;
                    break;
            }
        }
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
}
