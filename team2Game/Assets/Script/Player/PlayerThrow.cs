using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    private Rigidbody starRigid;            //Starオブジェクト用Rigidbody
    private Collider starCollider;          //Starオブジェクト用コライダー
    private Rigidbody rigid;                //Player用Rigidbody

    

    [SerializeField]
    private float throwPower = 10;          //投げる力

    [SerializeField]
    private float stopTime = 1;             //投げた後の停止時間
    public static float stopTimer;                //停止時間用タイマー

    public static bool dank;                      //下投げ

    private Animator animator;
    

    // Start is called before the first frame update
    void Start()
    {
        
        //Starオブジェクト用Rigidbody取得
        starRigid = GameManager.star.GetComponent<Rigidbody>();
        //Starオブジェクト用コライダー取得
        starCollider = GameManager.star.GetComponent<Collider>();
        
        //停止時間用タイマー初期化
        stopTimer = 0;
        //Starコライダー停止
        starCollider.enabled = false;

        //下投げ初期化
        dank = false;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UIManager.wishTimerFillamount = stopTimer;

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
                if (UIManager.hpGageFillAmount <= 20)
                {
                    UIManager.hpGageFillAmount = 20;
                }


                if (PlayerManager.isWishMode)
                {
                    PlayerManager.isWishMode = false;       //願い事モードを切る
                }
                else
                {
                    //Starオブジェクトを持っていない状態に
                    PlayerManager.haveStar = false;

                    //下投げでなければ
                    if (!dank)
                    {
                        //Playerの向いている方向に応じて投げる方向を変える
                        switch (PlayerManager.playerDirection)
                        {
                            //左を向いている場合
                            case PlayerManager.PlayerDirection.LEFT:
                                //starRigid.AddForce(new Vector3(-throwPower, -throwPower), ForceMode.Impulse);
                                starRigid.AddForce(PlayerManager.throwDirection * throwPower, ForceMode.Impulse);
                                break;
                            //右を向いている場合
                            case PlayerManager.PlayerDirection.RIGHT:
                                //starRigid.AddForce(new Vector3(throwPower, -throwPower), ForceMode.Impulse);
                                starRigid.AddForce(PlayerManager.throwDirection * throwPower, ForceMode.Impulse);
                                break;
                        }
                    }
                    //下投げの場合
                    else
                    {
                        //真下に投げる
                        starRigid.AddForce(new Vector3(0, -throwPower), ForceMode.Impulse);
                    }

                    //投げた瞬間の場所を格納
                    PlayerManager.throwPosition = GameManager.star.transform.position;

                    ////移動制限をかける
                    //playerManager.isStop = false;
                    UIManager.hpGageStopTimer = 0;

                    //Starコライダー開始
                    starCollider.enabled = true;
                }
            }
        }

        //Starオブジェクトを持っていた場合
        if (PlayerManager.haveStar)
        {
            //StarはPlayerの3マス上に
            GameManager.star.transform.position = transform.position + new Vector3(0, 2.5f);
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
            //もし願い事待機状態であれば
            if (PlayerManager.isWishStay)
            {
                stopTimer = -3f;        //移動制限用タイマーを1秒長くする
                PlayerManager.isWishMode = true;        //願い事モード起動
                PlayerManager.isWishStay = false;       //待機状態をfalseに
            }
            else
            {
                //移動制限用タイマー初期化
                stopTimer = 0;
                SoundManager.PlaySE(11);

                //Playerに触れられなくする
                GameManager.star.layer = 12;
                animator.SetTrigger("Throw");
            }
            //移動制限を開始
            PlayerManager.isStop = true;
        }
        //下投ボタンを押された場合
        if (Input.GetButtonDown("Fire2") && !PlayerManager.isStop)
        {
            //もし願い事待機状態であれば
            if (PlayerManager.isWishStay)
            {
                stopTimer = -3f;        //移動制限用タイマーを1秒長くする
                PlayerManager.isWishMode = true;        //願い事モード起動
                PlayerManager.isWishStay = false;       //待機状態をfalseに
            }
            else
            {
                //移動制限用タイマー初期化
                stopTimer = 0;
                SoundManager.PlaySE(11);
                //下投げをtrueに
                dank = true;

                //Playerに触れられなくする
                GameManager.star.layer = 12;
                animator.SetTrigger("Throw");
            }
            //移動制限を開始
            PlayerManager.isStop = true;
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

            SoundManager.PlaySE(2);
        }
    }
}
