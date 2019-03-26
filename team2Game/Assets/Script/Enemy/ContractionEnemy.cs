using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 体力を持ったEnemyクラス
/// </summary>
public class ContractionEnemy : Enemy
{
    //[SerializeField, Header("体力")]
    //int hp = 3;

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
        Contraction();
        Damage();
    }

    public override void Contraction()
    {
        transform.localScale = new Vector3(1, 1, 1) * GetScale();
    }

    float GetScale()
    {
        float scale;

        if (hp == 3) scale = 2;
        else if (hp == 2) scale = 1;
        else scale = 0.5f;

        return scale;
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
            rigid.AddForce(transform.forward * power, ForceMode.Acceleration);
        }
    }

    //public override void Damage()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        hp -= 1;
    //    }

    //    if (hp <= 0) Destroy(gameObject);
    //}

    public override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("Enemy"))
        {
            Direction_Left = !Direction_Left;
        }

        if (other.transform.tag == "Star")
        {
            hp--;
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
