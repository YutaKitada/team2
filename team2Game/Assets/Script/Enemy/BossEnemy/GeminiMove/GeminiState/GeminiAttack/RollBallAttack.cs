using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollBallAttack : MonoBehaviour　, IMove
{
    [SerializeField]
    private GameObject rollRock;        //岩プレハブ
    private bool isEndFlag;             //モーション終了判断

    [SerializeField]
    private GameObject playerPos;　　　　//プレイヤーの位置
    private Vector3 sabunPos;　　　　　　//差分計算用
    private Vector3 sabunVec;　　　　　　//プレイヤーがいる方向を取得

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

    public Move Next()
    {
        return Move.None;
    }

    void IMove.Update()
    {
        Roll();
    }
    void Roll()
    {
        gameObject.GetComponent<Renderer>().material.color
            = Color.red;
        Debug.Log("岩転がし");
        Instantiate(rollRock,
                    transform.position + new Vector3(3 * sabunVec.x, 1, 0),
                    new Quaternion(0, 0, 0, 0));
        isEndFlag = true;
    }
}
