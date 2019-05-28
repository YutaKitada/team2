using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowStorm : MonoBehaviour ,IMove
{

    [SerializeField]
    private GameObject snowStorm;          //風のプレハブ
    [SerializeField]
    private float maxTime =0;                 //最大効果時間
    private float currrentTime =0;            //経過時間

    private Animator anime;

    private bool isEndFlag;　　　　//モーション終了判断
    [SerializeField]
    private int seNumber = 0;
    public void Initialize()
    {
        isEndFlag = false;
        anime = GetComponent<Animator>();
        Instantiate(snowStorm,
                    new Vector3(0, 7, 0),
                    new Quaternion(0, 0, 0, 0));
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
        anime.SetBool("Storm",true);
        currrentTime += Time.deltaTime;
        SoundManager.PlaySE(seNumber);
        if (currrentTime>=maxTime)
        {
            anime.SetBool("Storm", false);
            currrentTime = 0;
            isEndFlag = true;
            
        }
        
    }
}
