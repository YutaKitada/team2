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
    [SerializeField]
    private float maxAttack;
    private float attacTime;

    [SerializeField]
    private GameObject attackArea;

    private bool isPunch;

    private bool isEndFlag;　　　　//モーション終了判断

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

        if (sabunAbs <=attackRange)
        {
            Hit();
           attacTime += Time.deltaTime;
            if (attacTime>=maxAttack)
            {
                Instantiate(attackArea,
                    transform.position + new Vector3(4, 1, 0),
                    new Quaternion(0, 0, 0, 0));
                curretTime = 0;
                isEndFlag = true;
                return;
            }
            
        }

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
