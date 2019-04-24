using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 体力に応じてスケールが変わるEnemyクラス（モチーフ：山羊座）
/// </summary>
public class ContractionEnemy : Enemy
{
    [SerializeField]
    List<float> hp_RayList;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rotation = Quaternion.identity;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        state = State.NORMAL;

        maxSpeed = power / 10f;
        changeDirection = GetComponent<ChangeDirection>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Direction();
        Contraction();
        SetTarget();
        Death();
    }

    public override void Contraction()
    {
        transform.localScale = new Vector3(1, 1, 1) * GetScale();
    }

    float GetScale()
    {
        float scale;

        if (hp == 3)
        {
            scale = 2.5f;
            changeDirection.MaxDistance = hp_RayList[0];
        }
        else if (hp == 2)
        {
            scale = 2f;
            changeDirection.MaxDistance = hp_RayList[1];
        }
        else
        {
            scale = 1.5f;
            changeDirection.MaxDistance = hp_RayList[2];
        }

        return scale;
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
