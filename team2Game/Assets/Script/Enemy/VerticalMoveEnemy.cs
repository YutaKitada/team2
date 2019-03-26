using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 縦移動のEnemyクラス
/// </summary>
public class VerticalMoveEnemy : Enemy
{
    private void Awake()
    {
        startPosition = transform.position.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Direction();
    }

    public override void Move()
    {
        if (Direction_Up)
        {
            rigid.AddForce(transform.up * power,ForceMode.Acceleration);
        }
        else
        {
            rigid.AddForce(-transform.up * power,ForceMode.Acceleration);
        }
    }

    public override void Direction()
    {
        //今のポジションが上目的地以下なら上に移動させる
        if (transform.position.y >= startPosition + destinationPosition)
        {
            Direction_Up = false;
        }
        //今のポジションが下目的地以上なら下に移動させる
        if (transform.position.y <= startPosition - destinationPosition)
        {
            Direction_Up = true;
        }
    }

    public override void OnCollisionEnter(Collision other)
    {
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
