using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public enum PlayerDirection { RIGHT, LEFT };      //プレイヤーの向き
    public static PlayerDirection playerDirection;         //プレイヤーの向き取得用

    public static bool isStop;                             //プレイヤーの移動制限
    public static bool haveStar;                  //Starを持っているか否か
    public static bool isWishStay;                  //願い事待機状態
    public static bool isWishMode;                  //願い事モード用Flag
    public static bool isInvincible;                //無敵
    private float invincibleTimer;                  //無敵時間用タイマー

    public static Vector3 throwPosition;            //投げた場所用Position

    public static Vector3 throwDirection;
    public static Vector3 effectDirection;

    private float directionPlus;
    [SerializeField]
    private float directionMoveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {

        playerDirection = PlayerDirection.RIGHT;    //最初は右向き
        isStop = false;             //移動制限なし
        haveStar = true;            //Starを持っている
        isWishStay = false;         //願い事待機状態ではない
        isWishMode = false;         //願い事モードではない
        isInvincible = false;       //無敵ではない
        invincibleTimer = 0;        //無敵用タイマー初期化

        throwPosition = Vector3.zero;   //一旦0に
        throwDirection = new Vector3(1, -1);
        effectDirection = new Vector3(1, -1);

        directionPlus = 0.5f;
    }

    private void Update()
    {
        //温度が80以上ある時、Rボタンを押すと願い事待機状態切り替え
        if(UIManager.hpGageFillAmount >= 75)
        {
            if (Input.GetButtonDown("YButton") && !WishManager.isWishNow)
            {
                if (!isWishMode)
                {
                    isWishMode = true;
                    PlayerThrow.stopTimer = -3f;        //移動制限用タイマーを1秒長くする
                    isStop = true;
                }
            }
        }
        //温度が80未満の場合、待機状態はfalseに
        else
        {
            isWishStay = false;
        }

        //無敵になった際、タイマー作動
        if (isInvincible)
        {
            invincibleTimer += Time.deltaTime;

            //タイマーが2秒以上カウントしたら無敵解除
            if(invincibleTimer >= 2f)
            {
                isInvincible = false;
                invincibleTimer = 0;
            }
        }

        ThrowDirection();
    }

    private void ThrowDirection()
    {
        //Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal_R"), Input.GetAxisRaw("Vertical_R"));

        if (playerDirection == PlayerDirection.LEFT)
        {
            if(directionPlus <= 1f && directionPlus >= 0f)
            {
                if(Input.GetAxisRaw("Horizontal_R") < 0)
                {
                    directionPlus += directionMoveSpeed * Time.deltaTime;
                }
                else if(Input.GetAxisRaw("Horizontal_R") > 0)
                {
                    directionPlus -= directionMoveSpeed * Time.deltaTime;
                }

                
            }
            if(directionPlus > 1) 
            {
                directionPlus = 1;
            }
            else if (directionPlus < 0)
            {
                directionPlus = 0;
            }

            
            throwDirection = Vector3.Lerp(new Vector3(-0.75f, -1), new Vector3(-2f, -1), directionPlus);
            effectDirection = Vector3.Lerp(new Vector3(-2f, -1), new Vector3(-0.75f, -1), directionPlus);

            throwDirection = throwDirection.normalized;
            effectDirection = effectDirection.normalized;
        }
        else if (playerDirection == PlayerDirection.RIGHT)
        {
            if (directionPlus <= 1f && directionPlus >= 0f)
            {
                if (Input.GetAxisRaw("Horizontal_R") > 0)
                {
                    directionPlus += directionMoveSpeed * Time.deltaTime;
                }
                else if (Input.GetAxisRaw("Horizontal_R") < 0)
                {
                    directionPlus -= directionMoveSpeed * Time.deltaTime;
                }


            }
            if (directionPlus > 1)
            {
                directionPlus = 1;
            }
            else if (directionPlus < 0)
            {
                directionPlus = 0;
            }

            throwDirection = Vector3.Lerp(new Vector3(0.75f, -1), new Vector3(2f, -1f), directionPlus);
            effectDirection = Vector3.Lerp(new Vector3(2f, -1), new Vector3(0.75f, -1), directionPlus);
            throwDirection = throwDirection.normalized;
            effectDirection = effectDirection.normalized;
        }
        //Debug.Log(direction);
    }


    //プレイヤーがダメージを受ける処理
    public static void PlayerDamage(float damage)
    {
        //無敵でなければ食らう
        if (!isInvincible)
        {
            UIManager.hpGageFillAmount -= damage;
            //無敵状態に
            isInvincible = true;
        }
        
    }
}
