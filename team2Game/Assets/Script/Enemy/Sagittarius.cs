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
    GameObject allow;
    Transform parent;

    [SerializeField]
    float range1 = 5;
    [SerializeField]
    float range2 = 5;

    Vector3 rangeVector1;
    Vector3 rangeVector2;
    bool isWithinShot = false;//射程圏内か
    bool isShot = false;//射程圏内に入ったときに撃たせるためのbool

    bool changeNow = false;

    Animator anim;

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

        anim = GetComponent<Animator>();

        parent = transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0)
            .transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0)
            .transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform;

        //子オブジェクトの位置に弾を生成
        allow = Instantiate(bullet, parent.position, transform.rotation);
        allow.transform.parent = parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.isWishMode || WishManager.wishProductionFlag) return;

        SetWIthin();
        Shot();
        Death();

        StartCoroutine(DirectionCoroutine());
    }

    /// <summary>
    /// 射程圏内に入ったかどうか
    /// </summary>
    void SetWIthin()
    {
        rangeVector1 = new Vector3(transform.position.x - range1, transform.position.y);//左側
        rangeVector2 = new Vector3(transform.position.x + range2, transform.position.y);//右側

        if ((target.position.x >= rangeVector1.x
            && target.position.x <= rangeVector2.x) && !changeNow)
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
            if (allow == null)
            {
                allow = Instantiate(bullet, parent.position, transform.rotation);
                allow.transform.parent = parent;
                return;
            }

            allow.GetComponent<Bullet>().IsShoot = true;
            anim.SetTrigger("shoot");
            elapsedTime = 0;
            isWithinShot = false;
            
            allow = Instantiate(bullet, parent.position, transform.rotation);
            allow.transform.parent = parent;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(rangeVector1, rangeVector1 + Vector3.up * 3);
        Gizmos.DrawLine(rangeVector2, rangeVector2 + Vector3.up * 3);
    }

    IEnumerator DirectionCoroutine()
    {
        float rate = 0;

        while (true)
        {
            rate += Time.deltaTime * 3;
            if(transform.position.x > target.position.x)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(forward), rate);
            }
            if (transform.position.x < target.position.x)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-forward), rate);
            }
            break;
        }

        if (GetAngle())
            changeNow = false;
        else changeNow = true;

        yield return null;
    }

    bool GetAngle()
    {
        bool isForward;

        if (transform.rotation == Quaternion.Euler(forward)
            || transform.rotation == Quaternion.Euler(-forward))
            isForward = true;
        else isForward = false;

        return isForward;
    }
}
