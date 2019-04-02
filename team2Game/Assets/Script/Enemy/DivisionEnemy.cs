using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 分裂するEnemy
/// </summary>
public class DivisionEnemy : Enemy
{
    Vector3 rightForce = new Vector3(10, 5, 0);
    Vector3 leftForce = new Vector3(-10, 5, 0);

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rotation = Quaternion.identity;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        state = State.NORMAL;

        maxSpeed = power / 10f;
    }

    private void Update()
    {
        Move();
        Direction();
        Contraction();
        //Damage();
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
                    rigid.AddForce(transform.forward * power, ForceMode.Acceleration);
                    break;
            }
        }
    }

    public override void Direction()
    {
        //左側を正面にする
        if (Direction_Left)
        {
            rotation = Quaternion.Euler(forward);
        }
        //右側を正面にする
        else
        {
            rotation = Quaternion.Euler(-forward);
        }
        //正面を進行方向にして移動
        transform.rotation = rotation;
    }

    public override void Contraction()
    {
        transform.localScale = new Vector3(1, 1, 1) * GetScale();
    }

    float GetScale()
    {
        float scale;

        if (hp == 3) scale = 1;
        else if (hp == 2) scale = 0.75f;
        else scale = 0.5f;

        return scale;
    }

    public override void Division()
    {
        Transform parent = transform.parent;
        GameObject obj;

        if (Direction_Left)
        {
            rigid.AddForce(rightForce * power, ForceMode.Acceleration);
            obj = Instantiate(this.gameObject, transform.position + Vector3.left, Quaternion.identity, parent);
            obj.GetComponent<Rigidbody>().AddForce(leftForce * power, ForceMode.Acceleration);
        }
        else
        {
            rigid.AddForce(leftForce * power, ForceMode.Acceleration);
            obj = Instantiate(this.gameObject, transform.position + Vector3.right, Quaternion.identity, parent);
            obj.GetComponent<Rigidbody>().AddForce(rightForce * power, ForceMode.Acceleration);
        }
        obj.GetComponent<Enemy>().Direction_Left = !Direction_Left;
    }

    public override void Damage()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
            hp -= 1;
            Division();
        //}
    }

    public override void SetTarget()
    {
        if (target.position.x - transform.position.x <= Mathf.Abs(5))
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
        //    Division();
        //    if (hp <= 0)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
    }
}
