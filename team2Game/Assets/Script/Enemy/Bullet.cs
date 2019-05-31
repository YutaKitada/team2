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

    public bool IsShoot//弾が撃たれたか
    {
        get;
        set;
    } = false;

    bool shootNow = false;

    Transform parent;

    bool isPlaySE = false;//SEが鳴ったか

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetPosition = target.position;
        initialPosition = transform.position;


        parent = transform.parent.transform.parent.transform.parent.transform.parent.transform.parent
            .transform.parent.transform.parent.transform.parent.transform.parent
            .transform.parent.transform.parent.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.isWishMode || WishManager.wishProductionFlag) return;

        if (!shootNow)
            shootVector = targetPosition - initialPosition + new Vector3(0, 1, 0);

        if (IsShoot)
        {
            if (!isPlaySE)
            {
                SoundManager.PlaySE(25);
                isPlaySE = true;
            }
            BulletMove();
        }
        else
        {
            Stop();
        }
    }

    void Stop()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    /// <summary>
    /// 弾の動き
    /// </summary>
    void BulletMove()
    {
        shootNow = true;
        transform.parent = null;
        rigid.AddForce(shootVector, ForceMode.Impulse);
        rigid.velocity /= 2;

        Destroy(gameObject, 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Playerに当たったら、ダメージを与え、後ろにノックバックさせる
        if(other.gameObject.tag=="Player")
        {
            PlayerManager.PlayerDamage(10);
            if (parent.GetComponent<Enemy>().direction_Left)
                other.GetComponent<PlayerController>().Damage(new Vector3(-5, 3));
            else
                other.GetComponent<PlayerController>().Damage(new Vector3(5, 3));

            Destroy(gameObject);
        }

        //他の矢や、射手座の敵以外に当たったら、削除
        if (!other.gameObject.name.Contains("Sagittarius") && !other.gameObject.name.Contains("Allow"))
        {
            Destroy(gameObject);
        }
    }
}
