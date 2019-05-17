using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeminiMoveManager : MonoBehaviour
{
    
    [SerializeField]
    private StateManager polStateManager;           //カストルのモーション管理場
    [SerializeField]
    private StateManager casStateManager;　　　　　 //ポルックスのモーション管理場

    private NoneState polNoneState;                 //NoneState判断用
    private NoneState casnoneState;

    [SerializeField]
    private float maxTime = 5;                 //最大効果時間
    private float currrentTime;            //経過時間
    private int num;                       //ランダムの値を代入

    // Start is called before the first frame update
    void Start()
    {
        polNoneState = polStateManager.GetComponent<NoneState>();
        casnoneState = casStateManager.GetComponent<NoneState>();
    }

    // Update is called once per frame
    void Update()
    {
        if (polNoneState.IsEnd() == false &&
            casnoneState.IsEnd() == false )
        {
            currrentTime += Time.deltaTime;
            Debug.Log("両待機状態");

            if (currrentTime >= maxTime)
            {
                num = Random.Range(0, 4);
                polNoneState.MoveNum(num);
                casnoneState.MoveNum(num);
                //攻撃方法3が選ばれたら片方を4にしてずらす。
                if(num ==3)
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
}
