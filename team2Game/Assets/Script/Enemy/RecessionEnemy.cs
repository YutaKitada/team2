using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 星を当てると後退するEnemyクラス
/// </summary>
public class RecessionEnemy : Enemy
{
    [SerializeField, Header("後退する距離")]
    float recessionDistance = 5f;
    Vector3 recessionVector;//後退するベクトル

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rotation = Quaternion.identity;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        state = State.NORMAL;

        maxSpeed = power / 10f;

        recessionVector = new Vector3(recessionDistance, 10, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Direction();
        SetTarget();
        //Damage();
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
                    rigid.AddForce(transform.forward * power, ForceMode.Acceleration);
                    break;

                case State.CHASE:
                    distance.x = target.position.x - transform.position.x;
                    if (distance.x < 0)
                    {
                        Direction_Left = true;
                    }
                    else if (distance.x > 0)
                    {
                        Direction_Left = false;
                    }
                    rigid.AddForce(transform.forward * power * 5, ForceMode.Acceleration);
                    break;
            }
        }
    }

    public override void Damage()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
            Vector3 recession;
            if (Direction_Left)
            {
                recession = recessionVector;
            }
            else
            {
                Vector3 reverceRecession = new Vector3(-recessionDistance, 10, 0);
                recession = reverceRecession;
            }
            rigid.AddForce(recession * power, ForceMode.Acceleration);
        //}
    }

    public override void SetTarget()
    {
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

    public override void OnCollisionEnter(Collision other)
    {
        //壁か別の敵に当たったとき進行方向を逆にする
        if (other.gameObject.tag.Contains("Stage")
            || other.gameObject.name.Contains("Enemy"))
        {
            Direction_Left = !Direction_Left;
        }

        //if (other.transform.tag == "Star")
        //{
        //    hp--;
        //    Vector3 recession;
        //    if (Direction_Left)
        //    {
        //        recession = recessionVector;
        //    }
        //    else
        //    {
        //        Vector3 reverceRecession = new Vector3(-recessionDistance, 10, 0);
        //        recession = reverceRecession;
        //    }
        //    rigid.AddForce(recession * power, ForceMode.Acceleration);
        //    if (hp <= 0)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
    }
}
