using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leo : Enemy
{
    [SerializeField]
    float maxDistance = 2;

    bool isAttack = false;//攻撃中か

    Animator anim;

    [SerializeField]
    GameObject hitParticle;

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

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttack)
        {
            //攻撃中でなければ移動
            Move();
        }
        Direction();
        SetTarget();
        StartCoroutine(AttackCoroutine());
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

    /// <summary>
    /// 終了検知を使用した攻撃
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackCoroutine()
    {
        if (ToForwardObject() != target && !isAttack) yield break;

        isAttack = true;
        anim.SetBool("isAttack", isAttack);

        var finish = StartCoroutine(FinishAnimation("attack"));
        yield return finish;

        if(ToForwardObject() == target)
            PlayerManager.PlayerDamage(10);
        isAttack = false;
        anim.SetBool("isAttack", isAttack);
    }

    /// <summary>
    /// アニメーションの終了検知
    /// </summary>
    /// <param name="animationName"></param>
    /// <returns></returns>
    IEnumerator FinishAnimation(string animationName)
    {
        bool finish = false;
        while(!finish)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack"))
            {
                finish = true;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Star" || collision.gameObject.tag == "Player")
        {
            rigid.constraints = RigidbodyConstraints.FreezeAll;
        }

        if (collision.gameObject.tag == "Star")
        {
            if(hp >= 1)
                Instantiate(hitParticle, collision.contacts[0].point, Quaternion.identity);
        }
    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag == "Star" || collision.gameObject.tag == "Player")
        {
            rigid.constraints = 
                RigidbodyConstraints.FreezePositionZ |
                RigidbodyConstraints.FreezeRotation;
        }
    }
}
