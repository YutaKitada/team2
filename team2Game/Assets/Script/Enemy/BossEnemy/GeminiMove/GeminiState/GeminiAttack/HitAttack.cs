using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAttack : MonoBehaviour , IMove
{
    [SerializeField]
    private GameObject playerPos;
    private float sabun;
    private float sabunAbs;            //差分の絶対値
    [SerializeField]
    private float attackRange =5; //攻撃有効範囲
    [SerializeField]
    private float maxTime =5;    //最大待機時間
    private float curretTime; //経過時間

    private bool isEndFlag;　　　　//モーション終了判断

    public void Initialize()
    {
        isEndFlag = true;
    }

    public bool IsEnd()
    {
        return isEndFlag;
    }

    public Move Next()
    {
        return Move.None;
    }

    void IMove.Update()
    {
        sabun = playerPos.transform.position.x - transform.position.x;
        sabunAbs = Mathf.Abs(sabun *-1);
        Debug.Log("力をためている");
        gameObject.GetComponent<Renderer>().material.color
            = Color.green;
        curretTime += Time.deltaTime;
        if(sabunAbs <=attackRange)
        {
            Hit();
            curretTime = 0;
            isEndFlag = true;
            
        }
        if(curretTime >= maxTime)
        {
            curretTime = 0;
            isEndFlag = true;
        }
    }
    /// <summary>
    /// アニメーションで殴る
    /// </summary>
    void Hit()
    {
        Debug.Log("パンチ！！");
        gameObject.GetComponent<Renderer>().material.color
            = Color.red;
    }
}
