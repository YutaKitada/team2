using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneState : MonoBehaviour , IMove
{
    [SerializeField]
    private float maxTime;                 //最大効果時間
    private float currrentTime;            //経過時間
    
    private int num;                       //ランダムの値を代入

    private bool isEndFlag;　　　　        //モーション終了判断

    public void Initialize()
    {
        isEndFlag = false;
    }

    public bool IsEnd()
    {
        return isEndFlag;
    }
    
    public Move Next()
    {
        return (Move)num;  　　　　　　　　//ランダムに代入されたnumをそのまま入れた
    }
    
    void IMove.Update()
    {
        Debug.Log("疲れた");
        //currrentTime += Time.deltaTime;
        
        //if (currrentTime >=maxTime)
        //{
        //    num = Random.Range(0, 5);
        //    currrentTime = 0;
        //    isEndFlag = true;
        //}
        
    }

    /// <summary>
    /// 強制終了ture
    /// </summary>
    /// <param name="end"></param>
    /// <returns></returns>
    public bool End( bool end)
    {
        return isEndFlag = end;
    }

    public int MoveNum(int moveNum)
    {
        return num = moveNum;
    }

    
}
