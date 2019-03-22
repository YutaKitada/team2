using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    private GameObject star;                //Starオブジェクト
    private Rigidbody starRigid;            //Starオブジェクト用Rigidbody
    private Collider starCollider;          //Starオブジェクト用コライダー
    private Rigidbody rigid;                //Player用Rigidbody

    

    [SerializeField]
    private float throwPower = 10;          //投げる力

    [SerializeField]
    private float stopTime = 1;             //投げた後の停止時間
    private float stopTimer;                //停止時間用タイマー

    private bool dank;
    

    // Start is called before the first frame update
    void Start()
    {
        //Starオブジェクト取得
        star = GameObject.FindGameObjectWithTag("Star");
        //Starオブジェクト用Rigidbody取得
        starRigid = star.GetComponent<Rigidbody>();
        //Starオブジェクト用コライダー取得
        starCollider = star.GetComponent<Collider>();
        
        //停止時間用タイマー初期化
        stopTimer = 0;
        //Starコライダー停止
        starCollider.enabled = false;
        dank = false;
    }

    // Update is called once per frame
    void Update()
    {
        //移動制限がかかった場合
        if (PlayerManager.isStop)
        {
            //タイマー作動
            stopTimer += Time.deltaTime;

            //タイマーが停止時間をうわまったら
            if (stopTimer >= stopTime)
            {
                //移動制限解除
                PlayerManager.isStop = false;

                //タイマー初期化
                stopTimer = 0;

                //ゲージが一定値以下の場合は増やす
                if (UIManager.gageFillAmount <= 20)
                {
                    UIManager.gageFillAmount = 20;
                }

                //Starオブジェクトを持っていない状態に
                PlayerManager.haveStar = false;

                if (!dank)
                {
                    //Playerの向いている方向に応じて投げる方向を変える
                    switch (PlayerManager.playerDirection)
                    {
                        //左を向いている場合
                        case PlayerManager.PlayerDirection.LEFT:
                            starRigid.AddForce(new Vector3(-throwPower, -throwPower), ForceMode.Impulse);
                            break;
                        //右を向いている場合
                        case PlayerManager.PlayerDirection.RIGHT:
                            starRigid.AddForce(new Vector3(throwPower, -throwPower), ForceMode.Impulse);
                            break;
                    }
                }
                else
                {
                    starRigid.AddForce(new Vector3(0, -throwPower), ForceMode.Impulse);
                    dank = false;
                }

                PlayerManager.throwPosition = star.transform.position;

                ////移動制限をかける
                //playerManager.isStop = false;
                UIManager.gageStopTimer = 0;

                //Starコライダー開始
                starCollider.enabled = true;
            }
        }

        //Starオブジェクトを持っていた場合
        if (PlayerManager.haveStar)
        {
            //StarはPlayerの3マス上に
            star.transform.position = transform.position + new Vector3(0, 2);
            //Starのvelocityを0に
            starRigid.velocity = Vector3.zero;

            //投げる処理
            Throw();
        }
        
        
    }

    private void Throw()
    {
        //投げるボタンを押された場合
        if (Input.GetButtonDown("Fire1") && !PlayerManager.isStop)
        {
            PlayerManager.isStop = true;
            stopTimer = 0;
            SoundManager.PlaySE(1);
            if (PlayerManager.isWishStay)
            {
                stopTimer = -1f;
                PlayerManager.isWishMode = true;
                PlayerManager.isWishStay = false;
            }
            star.layer = 12;
        }
        //投げるボタンを押された場合
        if (Input.GetButtonDown("Fire2") && !PlayerManager.isStop)
        {
            PlayerManager.isStop = true;
            stopTimer = 0;
            SoundManager.PlaySE(1);
            dank = true;
            star.layer = 12;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Starオブジェクトに当たった場合
        if(collision.transform.tag == "Star")
        {
            //Starオブジェクトを持っている状態に
            PlayerManager.haveStar = true;
            starCollider.enabled = false;

            SoundManager.PlaySE(0);
        }
    }
}
