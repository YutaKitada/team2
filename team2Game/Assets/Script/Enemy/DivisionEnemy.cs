using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 分裂するEnemy（モチーフ：双子座）
/// </summary>
public class DivisionEnemy : Enemy
{
    Vector3 rightForce = new Vector3(10, 5, 0);
    Vector3 leftForce = new Vector3(-10, 5, 0);

    Bound bound;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rotation = Quaternion.identity;

        bound = GetComponent<Bound>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        state = State.NORMAL;

        maxSpeed = power / 10f;
    }

    private void Update()
    {
        Move();
        Direction();
        Contraction();
        SetTarget();
        Death();
    }

    private void FixedUpdate()
    {
        SetBoundPower();
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

    public override void Contraction()
    {
        transform.localScale = new Vector3(1, 1, 1) * GetScale();
    }

    /// <summary>
    /// scale設定
    /// </summary>
    /// <returns></returns>
    float GetScale()
    {
        float scale;

        if (hp == 3) scale = 2f;
        else if (hp == 2) scale = 1f;
        else scale = 0.5f;

        return scale;
    }

    /// <summary>
    /// boundPower設定
    /// </summary>
    void SetBoundPower()
    {
        if (hp == 3) bound.BoundPower = 0.5f;
        else if (hp == 2) bound.BoundPower = 1f;
        else bound.BoundPower = 3f;
    }

    public override void Division()
    {
        Transform parent = transform.parent;
        GameObject obj;

        if (direction_Left)
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
        obj.GetComponent<Enemy>().direction_Left = !direction_Left;
    }

    public override void Damage()
    {
        hp -= 1;
        Division();
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

    public override void Death()
    {
        if (hp <= 0)
        {
            ParticleGenerate();
            Destroy(gameObject);
        }
    }
}
