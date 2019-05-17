using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾を撃つEnemyクラス（モチーフ：射手座）
/// </summary>
public class Sagittarius : Enemy
{
    float elapsedTime;//経過時間
    [SerializeField, Header("次の射撃までの待機時間")]
    float shotTime = 3;

    [SerializeField]
    GameObject bullet;//弾のprefab

    [SerializeField]
    float range1 = 5;
    [SerializeField]
    float range2 = 5;

    Vector3 rangeVector1;
    Vector3 rangeVector2;
    bool isWithinShot = false;//射程圏内か
    bool isShot = false;//射程圏内に入ったときに撃たせるためのbool

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

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
        Direction();
        SetWIthin();
        Shot();
        Death();
    }

    /// <summary>
    /// 射程圏内に入ったかどうか
    /// </summary>
    void SetWIthin()
    {
        rangeVector1 = new Vector3(transform.position.x - range1, transform.position.y);//左側
        rangeVector2 = new Vector3(transform.position.x + range2, transform.position.y);//右側

        if (target.position.x >= rangeVector1.x
            && target.position.x <= rangeVector2.x)
        {
            isWithinShot = true;
        }
    }

    public override void Shot()
    {
        if (!isWithinShot) return;

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= shotTime)
        {
            //子オブジェクトの位置に弾を生成
            Instantiate(bullet, transform.GetChild(0).position, transform.rotation);
            elapsedTime = 0;
            isWithinShot = false;
        }
    }

    public override void Direction()
    {
        //プレイヤーがいなければ何もしない
        if (target == null) return;
        else
        {
            distance.x = target.position.x - transform.position.x;
            if (distance.x < 0)
            {
                direction_Left = true;
            }
            else if (distance.x >= 0)
            {
                direction_Left = false;
            }
        }

        if(direction_Left)
        {
            rotation = Quaternion.Euler(forward);
        }
        else
        {
            rotation = Quaternion.Euler(-forward);
        }

        transform.rotation = rotation;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(rangeVector1, rangeVector1 + Vector3.up * 3);
        Gizmos.DrawLine(rangeVector2, rangeVector2 + Vector3.up * 3);
    }
}
