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

    private Animator anime;
    private float currentTime;
    [SerializeField]
    private float maxTime = 0.6f;

    public void Initialize()
    {
        isEndFlag = false;
        sabunPos = playerPos.transform.position - transform.position;    //プレイヤーの位置－自分の位置
        sabunVec = sabunPos.normalized;
        anime = GetComponent<Animator>();
        
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
        if (isEndFlag) return;
        Roll();
    }
    void Roll()
    {
        anime.SetBool("Korogari",true);
        currentTime += Time.deltaTime;
        
        if (currentTime >=maxTime)
        {
            Instantiate(rollRock,
                    transform.position + new Vector3(4 * sabunVec.x, 1.5f, 0),
                    new Quaternion(0, 0, 0, 0));
            currentTime = 0;
            isEndFlag = true;
            anime.SetBool("Korogari", false);
        }

        
    }
}
