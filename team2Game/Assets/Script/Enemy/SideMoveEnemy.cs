using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 横移動のEnemyクラス
/// </summary>
public class SideMoveEnemy : Enemy
{
    private void Awake()
    {
        startPosition = transform.position.x;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rotation = Quaternion.identity;

        maxSpeed = power / 10f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Direction();
    }

    public override void Move()
    {
        //今のスピードを計算
        float nowSpeed = Mathf.Abs(rigid.velocity.x);

        //最大の移動スピードを超えていないとき
        if (nowSpeed < maxSpeed)
        {
            //ターゲットがいるとき
            if (target != null)
            {
                distance.x = target.position.x - transform.position.x;
                if (distance.x < 0)
                {
                    Direction_Left = true;
                }
                else if (distance.x > 0)
                {
                    Direction_Left = false;
                }
            }
            rigid.AddForce(transform.forward * power, ForceMode.Acceleration);
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

    private void OnDrawGizmosSelected()
    {
        if (Direction_Left)
        {
            Gizmos.DrawLine(transform.position, transform.position + (Vector3.down + Vector3.left));
        }
        else
        {
            Gizmos.DrawLine(transform.position, transform.position + (Vector3.down + Vector3.right));
        }
    }

    public override void OnCollisionEnter(Collision other)
    {
        //壁か別の敵に当たったとき進行方向を逆にする
        if (other.gameObject.name.Contains("Wall")
            || other.gameObject.name.Contains("Enemy"))
        {
            Direction_Left = !Direction_Left;
        }

        if(other.transform.tag == "Star")
        {
            hp--;
            if(hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
