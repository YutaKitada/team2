﻿using System.Collections;
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
        Shot();
        Death();
    }

    public override void Shot()
    {
        //プレイヤーがいなければ何もしない
        if (target == null) return;

        elapsedTime += Time.deltaTime;
        if(elapsedTime >= shotTime)
        {
            //子オブジェクトの位置に弾を生成
            Instantiate(bullet, transform.GetChild(0).position, transform.rotation);
            elapsedTime = 0;
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
}