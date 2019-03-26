using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 分裂するEnemy
/// </summary>
public class DivisionEnemy : Enemy
{
    //[SerializeField, Header("体力")]
    //int hp = 3;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Direction();
        Contraction();
        Damage();
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
        GameObject obj = Instantiate(this.gameObject, transform.position, Quaternion.identity, parent);
    }

    //public override void Damage()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        hp -= 1;

    //        transform.position += -transform.forward;
    //        Division();
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
            transform.position += -transform.forward;
            Division();
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
