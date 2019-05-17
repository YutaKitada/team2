using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupSelect : MonoBehaviour
{
    private bool isMove;
    private bool isMin;
    private bool isMax;

    private int pushCount =0;

    // Start is called before the first frame update
    void Start()
    {
        isMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        pushCount = Mathf.Clamp(pushCount, 0, 2);
        Scroll();
        IsEnd();
    }

    void Scroll()
    {
        //アローキーの上下に移動する
        if (LeftStick.Instance.IsTopDown() && !isMin)
        {
            isMax = false;
            isMove = true;
            pushCount++;
            if (pushCount==2)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y -450, transform.localPosition.z);
            }

        }

        if (LeftStick.Instance.IsBottomDown() && !isMax)
        {
            isMin = false;
            isMove = true;
            pushCount--;
            if (pushCount ==1)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 450, transform.localPosition.z);
            }
            
        }
    }
    //端かどうかの判断
    void IsEnd()
    {
        if (pushCount <=0)
        {
            isMax = true;
            isMove = false;
        }
        if (pushCount >=2)
        {
            isMin = true;
            isMove = false;
        }
    }

}
