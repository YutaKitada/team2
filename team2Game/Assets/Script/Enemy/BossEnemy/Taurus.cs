﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボス（モチーフ：牡牛座）
/// </summary>
public class Taurus : BossEnemy
{
    [SerializeField, Header("移動力")]
    float power = 20;

    [SerializeField, Header("突進までの待機時間")]
    float interval = 5;
    float intervalElapsedTime;//待機中の経過時間

    float deadElapsedTime;//死亡からの経過時間

    //全ての状態
    public enum Mode
    {
        WAIT,
        RUSH,
        STAN,
        INVINCIBLE
    }
    [HideInInspector]
    public Mode mode;//現在の状態

    Vector3 targetPosition;//突進開始時のプレイヤーの位置
    Vector3 startPosition;//突進開始地点

    bool isChange = false;//方向転換中か

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rotation = Quaternion.identity;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        mode = Mode.WAIT;//待機状態に設定

        //全ての経過時間を0
        stanElapsedTime = 0;
        invincibleElapsedTime = 0;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead)
        {
            //死亡してから、アニメーションが終わるまでのおおよその時間経過でパーティクル生成、
            //かつ、return以下の処理を行わない
            deadElapsedTime += Time.deltaTime;
            if (deadElapsedTime >= 2)
            {
                Instantiate(downParticle, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            return;
        }

        switch (mode)//状態に応じた処理を実行
        {
            case Mode.WAIT:
                //Direction();
                StartCoroutine(DirectionCoroutine());
                RushPrepare();
                anim.speed = 1;
                break;

            case Mode.RUSH:
                RushAttack();
                break;

            case Mode.STAN:
                NowStan();
                anim.speed = 0;
                break;

            case Mode.INVINCIBLE:
                NowInvincible();
                break;

            default:
                break;
        }

        Death();
    }

    public override void Direction()
    {
        if(transform.position.x > target.position.x)
        {
            rotation = Quaternion.Euler(forward);
        }
        if (transform.position.x < target.position.x)
        {
            rotation = Quaternion.Euler(-forward);
        }
        transform.rotation = rotation;
    }

    /// <summary>
    /// 突進準備
    /// </summary>
    void RushPrepare()
    {
        intervalElapsedTime += Time.deltaTime;
        //待機中の経過時間が規定時間まで過ぎたら突進状態へ移行
        if (intervalElapsedTime < interval) return;
        else
        {
            if (isChange) return;

            //突進に必要な情報を得る
            startPosition = transform.position;
            targetPosition = target.position;
            intervalElapsedTime = 0;
            mode = Mode.RUSH;
        }
    }

    /// <summary>
    /// 突進攻撃
    /// </summary>
    void RushAttack()
    {
        rigid.AddForce(transform.forward * power, ForceMode.Acceleration);
        //rigid.velocity /= 2f;
    }

    /// <summary>
    /// スタン中の処理
    /// </summary>
    void NowStan()
    {
        stanElapsedTime += Time.deltaTime;
        if (stanElapsedTime >= stanTime)
        {
            stanElapsedTime = 0;
            mode = Mode.WAIT;
        }
    }

    /// <summary>
    /// 無敵時間の処理
    /// </summary>
    void NowInvincible()
    {
        invincibleElapsedTime += Time.deltaTime;

        if(invincibleElapsedTime >= invincibleTime)
        {
            invincibleElapsedTime = 0;
            mode = Mode.WAIT;
            isHit = true;

            intervalElapsedTime = 0;
        }
    }

    public override void OnCollisionEnter(Collision other)
    {
        //Playerに当たったらPlayerにダメージ
        if (other.gameObject.tag.Contains("Player"))
        {
            //その場にとどまり、待機状態に移行
            Stop();
            PlayerManager.PlayerDamage(10);
            mode = Mode.WAIT;
        }

        //壁に当たったらスタン状態に移行
        if (other.gameObject.name.Contains("Wall"))
        {
            Stop();
            mode = Mode.STAN;
        }
    }

    /// <summary>
    /// 時間をかけて回転させる
    /// </summary>
    /// <returns></returns>
    IEnumerator DirectionCoroutine()
    {
        float rate = 0;

        while (true)
        {
            rate += Time.deltaTime * 3;
            if (transform.position.x > target.position.x)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(forward), rate);
            }
            if (transform.position.x < target.position.x)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-forward), rate);
            }
            isChange = true;
            break;
        }

        //Playerの方向に向き終わるまでは方向転換中に設定
        if (transform.rotation == Quaternion.Euler(forward) ||
            transform.rotation == Quaternion.Euler(-forward))
        {
            isChange = false;
        }

        yield return null;
    }
}
