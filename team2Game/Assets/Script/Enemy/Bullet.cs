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

    //プレイヤー
    Transform target;
    Vector3 targetPosition;

    Vector3 initialPosition;//初期位置
    Vector3 shootVector;//弾の方向

    public bool IsShoot
    {
        get;
        set;
    } = false;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetPosition = target.position;
        initialPosition = transform.position;
        shootVector = targetPosition - initialPosition + new Vector3(0, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(IsShoot)
            BulletMove();
    }

    /// <summary>
    /// 弾の動き
    /// </summary>
    void BulletMove()
    {
        rigid.AddForce(shootVector, ForceMode.Impulse);

        Destroy(gameObject, 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            PlayerManager.PlayerDamage(10);
        }
    }
}
