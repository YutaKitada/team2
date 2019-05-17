﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ThrowAttack : MonoBehaviour, IMove
{
    [SerializeField]
    private GameObject snowBall;   //雪玉プレハブ
    [SerializeField]
    private float interval = 3;　　　　//インターバルカウント　変更可
    private float currntInterval;　//現在のインターバル時間
    [SerializeField]
    private int throwCountMax = 3;　　 //投げる回数
    private int currentThrowCount; //現在の投げた回数

    [SerializeField]
    private GameObject playerPos;　　　　//プレイヤーの位置
    private Vector3 sabunPos;　　　　　　//差分計算用
    private Vector3 sabunVec;      //プレイヤーがいる方向を取得

    private bool isEndFlag;　　　　//モーション終了判断
    public void Initialize()
    {
        isEndFlag = false;
        sabunPos = playerPos.transform.position - transform.position;    //プレイヤーの位置－自分の位置
        sabunVec = sabunPos.normalized;
    }

    public bool IsEnd()
    {
        return isEndFlag;
    }
    /// <summary>
    /// 次のモーション
    /// </summary>
    /// <returns></returns>
    public Move Next()
    {
        return Move.None;
    }


    void IMove.Update()
    {
        currntInterval += Time.deltaTime;
        if(currntInterval >= interval)
        {
            Throw();
            currntInterval = 0;
            currentThrowCount++;
        }
        //攻撃回数を決めて終わったらNoneの状態にする
        if(currentThrowCount >= throwCountMax)
        {
            isEndFlag = true;
        }
        
    }
    /// <summary>
    /// 雪玉生成
    /// </summary>
    void Throw()
    {
        gameObject.GetComponent<Renderer>().material.color
            = Color.blue;
        Debug.Log("雪投げ");
        Instantiate(snowBall,
                    transform.position + new Vector3(1.5f * sabunVec.x, 1, 0),
                    new Quaternion(0, 0, 0, 0));
    }
}