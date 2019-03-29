using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆発するEnemyクラス
/// </summary>
public class ExplosionEnemy : Enemy
{
    [SerializeField]
    GameObject particle;

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
        Damage();
        Death();
    }

    public override void Move()
    {

    }

    public override void Direction()
    {

    }

    public override void Explosion()
    {
        Transform parent = transform.parent;
        GameObject obj = Instantiate(particle, transform.position, Quaternion.identity, parent);
    }

    public override void Damage()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        hp--;
        Explosion();
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
        //    Explosion();
        //    if (hp <= 0)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
    }
}
