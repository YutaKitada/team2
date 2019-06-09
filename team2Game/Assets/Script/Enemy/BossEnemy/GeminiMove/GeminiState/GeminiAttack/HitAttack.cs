using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAttack : MonoBehaviour , IMove
{
    [SerializeField]
    private GameObject playerPos;　　　//プレイヤーの位置取得
    private float sabun;
    private float sabunAbs;            //差分の絶対値
    [SerializeField]
    private float attackRange =5;　　 //攻撃有効範囲
    [SerializeField]
    private float maxTime =3;    　　 //最大待機時間
    private float curretTime; 　　　　//経過時間
    private Animator anime;　　　　　 //アニメーション
    [SerializeField]
    private float maxAttack;　　　　　//最大待機時間
    private float attackTime;　　　　 //現在経過時間

    [SerializeField]
    private GameObject attackArea;    //有効攻撃範囲

    private bool isPunch;　　　　　　 //パンチするかどうか

    private bool isEndFlag;　　　　　//モーション終了判断

    public void Initialize()
    {
        isEndFlag = false;
        anime = GetComponent<Animator>();
        anime.SetTrigger("Tame");
        isPunch = false;
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
        //攻撃有効範囲よりプレイヤーが中にいるのなら殴る
        if (sabunAbs <=attackRange)
        {
            Hit();
          　attackTime += Time.deltaTime;
            if (attackTime>=maxAttack)
            {
                //攻撃プレハブを生成
                Instantiate(attackArea,
                    transform.position + new Vector3(4, 1, 0),
                    new Quaternion(0, 0, 0, 0));
                curretTime = 0;
                isEndFlag = true;
                return;
            }
        }
        //時間でNone状態へ遷移
        if (curretTime >= maxTime)
        {
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
        if (isPunch) { return; }
        isPunch = true;
        anime.SetTrigger("Panch2");
        
    }
}
