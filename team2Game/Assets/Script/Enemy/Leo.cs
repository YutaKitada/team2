using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leo : Enemy
{
    float attackInterval = 1.5f;
    float intervalElapsedTime;

    [SerializeField]
    float maxDistance = 2;

    bool isAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rotation = Quaternion.identity;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        state = State.NORMAL;

        maxSpeed = power / 10f;
        changeDirection = GetComponent<ChangeDirection>();
        changeDirection.MaxDistance = maxDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttack)
        {
            Move();
        }
        Direction();
        SetTarget();
        Attack();
        Death();
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

    /// <summary>
    /// 攻撃処理
    /// </summary>
    void Attack()
    {
        //正面のオブジェクトがプレイヤー以外であれば、return
        if (ToForwardObject() != target && !isAttack) return;

        isAttack = true;

        //時間でダメージを与える
        intervalElapsedTime += Time.deltaTime;
        Debug.Log(intervalElapsedTime);
        if (intervalElapsedTime >= attackInterval)
        {
            //正面にまだプレイヤーがいる場合のみダメージを与える
            if (ToForwardObject() == target)
            {
                PlayerManager.PlayerDamage(10);
            }
            intervalElapsedTime = 0;
            isAttack = false;
        }
    }

    /// <summary>
    /// 正面にあるオブジェクトを取得
    /// </summary>
    /// <returns></returns>
    Transform ToForwardObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, maxDistance))
        {
            return hit.transform;
        }

        return null;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * maxDistance);
    }
}
