using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimUI : MonoBehaviour
{
    private GameObject starObject;
    public GameObject playerAimUI;

    // Start is called before the first frame update
    void Start()
    {
        starObject = GameObject.FindGameObjectWithTag("Star");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.playerDirection == PlayerManager.PlayerDirection.LEFT)
        {
            //playerAimUI.transform.localPosition = new Vector3(-2.5f, -1);

            //Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向
            Ray ray = new Ray(starObject.transform.position, new Vector3(-1, -1, 0));

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
                playerAimUI.transform.position = hit.point;
            }
        }
        else if (PlayerManager.playerDirection == PlayerManager.PlayerDirection.RIGHT)
        {
            //playerAimUI.transform.localPosition = new Vector3(2.5f, -1);

            //Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向
            Ray ray = new Ray(starObject.transform.position, new Vector3(1, -1, 0));

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
                playerAimUI.transform.position = hit.point;
            }
        }

        if (PlayerManager.haveStar && !playerAimUI.activeSelf)
        {
            playerAimUI.SetActive(true);
        }
        else if (!PlayerManager.haveStar && playerAimUI.activeSelf)
        {
            playerAimUI.SetActive(false);
        }
    }
}
