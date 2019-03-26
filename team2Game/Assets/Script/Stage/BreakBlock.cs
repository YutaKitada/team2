using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour
{

    [SerializeField]
    private float breakDistance = 3;        //壊れる距離
    

    private void OnTriggerEnter(Collider collision)
    {
        //Starに当たったら
        if(collision.transform.tag == "Star")
        {
            //跳ね返った後に当たっていなければ
            if (!GameManager.star.GetComponent<StarMovement>().returnPlayer)
            {
                //壊れる距離が0でなければ
                if (breakDistance != 0)
                {
                    //距離の取得
                    float distace = Vector3.Distance(PlayerManager.throwPosition, transform.position);
                    //投げた場所から自分のいる位置までの長さがbreakDistanceを超えていれば壊れる
                    if (distace >= breakDistance)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
