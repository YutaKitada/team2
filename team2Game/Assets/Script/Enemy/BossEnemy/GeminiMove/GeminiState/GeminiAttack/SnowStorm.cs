using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowStorm : MonoBehaviour ,IMove
{

    [SerializeField]
    private GameObject snowStorm;          //風のプレハブ
    [SerializeField]
    private float maxTime;                 //最大効果時間
    private float currrentTime;            //経過時間

    private bool isEndFlag;　　　　//モーション終了判断

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
        return Move.None;
    }

    void IMove.Update()
    {
        Storm();
    }
    void Storm()
    {
        gameObject.GetComponent<Renderer>().material.color
            = Color.gray;
        Debug.Log("吹雪");
        Instantiate(snowStorm,
                    new Vector3(0, 7, 0),
                    new Quaternion(0, 0, 0, 0));
        isEndFlag = true;
    }
}
