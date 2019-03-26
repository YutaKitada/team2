using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBlock : MonoBehaviour
{
    [SerializeField]
    private GameObject block;       //板ブロック
    

    private void OnTriggerStay(Collider other)
    {
        //Playerに触れたら
        if (other.tag == "Player")
        {
            //板ブロックをPlayerに触れられるようにする
            block.layer = 11;
            //もし下方向に入力があった場合
            if(Input.GetAxisRaw("Vertical") <= -0.7f)
            { 
                //板ブロックをPlayerに触れられないようにする
                block.layer = 12;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Playerが離れたら
        if (other.tag == "Player")
        {
            //板ブロックをPlayerに触れられないようにする
            block.layer = 12;
        }
    }
}
