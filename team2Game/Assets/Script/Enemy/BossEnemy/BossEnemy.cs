﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BossEnemy : MonoBehaviour
{
    protected Rigidbody rigid;
    protected Quaternion rotation;

    [SerializeField]
    protected int hp = 5;

    [SerializeField, Header("スタンの持続時間")]
    protected float stanTime = 3;
    protected float stanElapsedTime;//スタン中の経過時間

    [HideInInspector]
    public bool isHit = true;//プレイヤーの攻撃が当たるかどうか
    [SerializeField, Header("無敵時間")]
    protected float invincibleTime = 3;
    protected float invincibleElapsedTime;//無敵時間の経過時間

    protected Transform target;

    protected Animator anim;

    protected Vector3 forward = new Vector3(0, -90, 0);//正面

    [SerializeField]
    protected GameObject downParticle;

    protected float deadElapsedTime;//死亡からの経過時間

    public bool IsDead
    {
        get;
        protected set;
    } = false;

    [SerializeField]
    float scale = 100;

    public bool OnRight
    {
        get;
        set;
    }

    /// <summary>
    /// ダメージ
    /// </summary>
    public virtual void Damage(int damage)
    {
        if (!isHit) return;

        isHit = false;
        hp -=damage;
    }

    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        rigid.constraints = RigidbodyConstraints.FreezeAll;
    }

    public virtual void Death()
    {
        if (IsDead) return;

        if (hp <= 0)
        {
            IsDead = true;
            if (anim != null)
            {
                AnimReverse();

                anim.SetTrigger("isDead");
                anim.speed = 1;
            }
        }
    }

    public virtual void OnCollisionEnter(Collision other)
    {

    }

    /// <summary>
    /// アニメーション反転させるためのscaleいじり
    /// </summary>
    void AnimReverse()
    {
        if (OnRight)
        {
            transform.localScale = new Vector3(-scale, scale, scale);
        }
        else
        {
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
