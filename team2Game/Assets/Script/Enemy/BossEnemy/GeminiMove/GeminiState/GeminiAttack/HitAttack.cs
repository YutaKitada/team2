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
    private float maxTime =3;    //最大待機時間
    private float curretTime; //経過時間
    private Animator anime;

    private bool isEndFlag;　　　　//モーション終了判断

    public void Initialize()
    {
        isEndFlag = true;
        anime = GetComponent<Animator>();
        Debug.Log("力をためている");
        anime.SetTrigger("Tame");
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
        curretTime += Time.deltaTime;
        if(sabunAbs <=attackRange)
        {
            Hit();
            curretTime = 0;
            isEndFlag = true;
            
        }
        else if(curretTime >= maxTime)
        {
            Debug.Log("パンチ失敗");
            anime.SetTrigger("NotPanch");
            curretTime = 0;
            isEndFlag = true;
        }
    }
    /// <summary>
    /// アニメーションで殴る
    /// </summary>
    void Hit()
    {
        anime.SetTrigger("Panch");
        Debug.Log("パンチ！！");
        
    }
}
