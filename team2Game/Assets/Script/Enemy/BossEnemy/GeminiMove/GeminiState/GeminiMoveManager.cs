using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeminiMoveManager : MonoBehaviour
{
    
    [SerializeField]
    private StateManager polStateManager;           //ポルックスのモーション管理場
    [SerializeField]
    private StateManager casStateManager;　　　　　 //カストルのモーション管理場

    private NoneState polNoneState;                 //ポルックスNoneState判断用
    private NoneState casnoneState;　　　　　　　　 //カストルNoneState判断用

    [SerializeField]
    private float maxTime = 5;                      //最大効果時間
    private float currrentTime;                     //経過時間
    private int num;                                //ランダムの値を代入

    // Start is called before the first frame update
    void Start()
    {
        polNoneState = polStateManager.GetComponent<NoneState>();
        casnoneState = casStateManager.GetComponent<NoneState>();
    }

    // Update is called once per frame
    void Update()
    {
        GeminiState();
        OneDeath();
        OneAttack();
    }

    /// <summary>
    /// ふたご座のそれぞれの状態を管理
    /// </summary>
    void GeminiState()
    {
        //二人ともNone状態の時経過時間を＋＋する
        if (polNoneState.IsEnd() == false &&
            casnoneState.IsEnd() == false)
        {
            currrentTime += Time.deltaTime;
            //４つの攻撃方法の中からランダムに選択
            if (currrentTime >= maxTime)
            {
                num = Random.Range(0, 4);
                polNoneState.MoveNum(num);
                casnoneState.MoveNum(num);
                //攻撃方法3が選ばれたら片方を4にしてずらす。
                if (num == 3)
                {
                    casnoneState.MoveNum(4);
                }

                //None状態からnum番号へ移行
                polNoneState.End(true);
                casnoneState.End(true);

                currrentTime = 0;
            }
        }
        
    }
    /// <summary>
    /// どちらかが倒れたらmaxtimeを1にして攻撃回転を速める
    /// </summary>
    void OneDeath()
    {
        if(casStateManager ==null|| polStateManager ==null)
        {
            maxTime = 1;
        }
    }

    //一人が死んだ時の処理
    void OneAttack()
    {
        if (polStateManager == null)
        {
            currrentTime += Time.deltaTime;
            if (currrentTime >= maxTime)
            {
                num = Random.Range(0, 3);
                
                casnoneState.MoveNum(num);

                //None状態からnum番号へ移行
                
                casnoneState.End(true);

                currrentTime = 0;
            }
        }

        if (casStateManager == null)
        {
            currrentTime += Time.deltaTime;
            if (currrentTime >= maxTime)
            {
                num = Random.Range(0, 3);
                polNoneState.MoveNum(num);
                
                //None状態からnum番号へ移行

                polNoneState.End(true);

                currrentTime = 0;
            }
        }
    }
}
