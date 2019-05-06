using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimUI : MonoBehaviour
{
    [SerializeField]
    private GameObject playerAimUI;      //照準用UIの取得
    

    // Update is called once per frame
    void Update()
    {
        //左を向いていれば
        //if (PlayerManager.playerDirection == PlayerManager.PlayerDirection.LEFT)
        //{
        //playerAimUI.transform.localPosition = new Vector3(-2.5f, -1);

        //Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向
        //Ray ray = new Ray(GameManager.star.transform.position, new Vector3(-1, -1, 0));
        Ray ray = new Ray(GameManager.star.transform.position, PlayerManager.throwDirection);

        //Rayが当たったオブジェクトの情報を入れる箱
        RaycastHit hit;

            //Rayの飛ばせる距離
            int distance = 100;

            //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　　　　　↓Rayの色
            Debug.DrawLine(ray.origin, ray.direction * distance, Color.red);

            //もしRayにオブジェクトが衝突したら
            //                  ↓Ray  ↓Rayが当たったオブジェクト ↓距離
            if (Physics.Raycast(ray, out hit, distance))
            {
                playerAimUI.transform.position = hit.point + new Vector3(0, 0, 0);
            }
        //}
        ////右を向いていれば
        //else if (PlayerManager.playerDirection == PlayerManager.PlayerDirection.RIGHT)
        //{
        //    //playerAimUI.transform.localPosition = new Vector3(2.5f, -1);

        //    //Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向
        //    Ray ray = new Ray(GameManager.star.transform.position, new Vector3(1, -1, 0));

        //    //Rayが当たったオブジェクトの情報を入れる箱
        //    RaycastHit hit;

        //    //Rayの飛ばせる距離
        //    int distance = 100;

        //    //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　　　　　↓Rayの色
        //    Debug.DrawLine(ray.origin, ray.direction * distance, Color.red);

        //    //もしRayにオブジェクトが衝突したら
        //    //                  ↓Ray  ↓Rayが当たったオブジェクト ↓距離
        //    if (Physics.Raycast(ray, out hit, distance))
        //    {
        //        playerAimUI.transform.position = hit.point + new Vector3(0,0,0);
        //    }
        //}

        //Starを持っている状態で、AimUIがfalseであれば
        if (PlayerManager.haveStar && !playerAimUI.activeSelf)
        {
            playerAimUI.SetActive(true);        //activeSelfをtrueに
        }
        //Starを持っていない状態で、AimUIがtrueであれば
        else if (!PlayerManager.haveStar && playerAimUI.activeSelf)
        {
            playerAimUI.SetActive(false);       //activeSelfをfalseに
        }

        if (PlayerManager.isWishMode)
        {
            playerAimUI.SetActive(false);       //activeSelfをfalseに
        }
    }
}
