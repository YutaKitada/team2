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

    }

    public override void Direction()
    {
        rotation = Quaternion.Euler(-forward);

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

    public override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("Enemy"))
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
