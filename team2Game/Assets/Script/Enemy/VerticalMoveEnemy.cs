using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 縦移動のEnemyクラス（モチーフ：牡羊座）
/// </summary>
public class VerticalMoveEnemy : Enemy
{
    [SerializeField, Header("目的地")]
    Vector3 targetPosition;
    Vector3 startPosition;//初期位置

    Vector3 destinationPosition1;
    Vector3 destinationPosition2;

    [SerializeField, Header("スピード")]
    float speed = 1;

    private void Awake()
    {
        startPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        destinationPosition1 = startPosition + targetPosition;
        destinationPosition2 = startPosition - targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Direction();
        Death();
    }

    public override void Move()
    {
        if (direction_Up)
        {
            //rigid.velocity = Vector3.up * power;
            rigid.AddForce(Vector3.up, ForceMode.Acceleration);
            if (rigid.velocity.y >= speed)
                rigid.velocity = new Vector3(0, speed, 0);
        }
        else
        {
            //rigid.velocity = Vector3.down * power;
            rigid.AddForce(Vector3.down, ForceMode.Acceleration);
            if (rigid.velocity.y <= -speed)
                rigid.velocity = new Vector3(0, -speed, 0);
        }
    }

    public override void Direction()
    {
        //今のポジションが上目的地以上なら下移動に変更
        if (transform.position.y >= destinationPosition1.y)
        {
            direction_Up = false;
        }
        //今のポジションが下目的地以下なら上移動に変更
        if (transform.position.y <= destinationPosition2.y)
        {
            direction_Up = true;
        }
    }
    
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag.Contains("Stage"))
        {
            direction_Up = !direction_Up;
        }
    }
}
