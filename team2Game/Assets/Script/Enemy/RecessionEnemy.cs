using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージを受けると後退するEnemyクラス
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

        maxSpeed = power / 10f;

        recessionVector = new Vector3(recessionDistance, 10, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Direction();
        //Damage();
    }

    public override void Move()
    {

    }

    public override void Direction()
    {
        if(Direction_Left)
        {
            rotation = Quaternion.Euler(forward);
        }
        else
        {
            rotation = Quaternion.Euler(-forward);
        }
        transform.rotation = rotation;
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

    public override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("Enemy"))
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
