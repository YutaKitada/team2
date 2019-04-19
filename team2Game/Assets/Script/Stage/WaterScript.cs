using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        //Starが触れている場合
        if (other.tag == "Star")
        {
            if (!PlayerManager.isWishMode)
            {
                UIManager.hpGageFillAmount -= 10f * Time.deltaTime;       //体力を毎秒10減らす
            }
            
        }
    }
}
