using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の挙動のクラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    Rigidbody rigid;
    [SerializeField, Header("弾の速度")]
    float power = 2;

    Transform target;
    Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetPosition = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        BulletMove();
    }

    void BulletMove()
    {
        rigid.AddForce(targetPosition, ForceMode.Impulse);

        Destroy(gameObject, 2);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag=="Player")
        {
            PlayerManager.PlayerDamage(10);
            //Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }
}
