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
    public static bool isWishStay;
    public static bool isWishMode;
    public static bool isInvincible;
    private float invincibleTimer;

    public static float starDistance;
    public static Vector3 throwPosition;

    // Start is called before the first frame update
    void Start()
    {

        playerDirection = PlayerDirection.RIGHT;    //最初は右向き
        isStop = false;
        haveStar = true;
        isWishStay = false;
        isWishMode = false;
        isInvincible = false;
        invincibleTimer = 0;
        starDistance = 0;
        throwPosition = Vector3.zero;
    }

    private void Update()
    {
        if(UIManager.gageFillAmount >= 80)
        {
            if (Input.GetButtonDown("RButton"))
            {
                if (!isWishStay)
                {
                    isWishStay = true;
                }
                else if (isWishStay)
                {
                    isWishStay = false;
                }
            }
        }
        else
        {
            isWishStay = false;
        }

        if (isInvincible)
        {
            invincibleTimer += Time.deltaTime;
            if(invincibleTimer >= 2f)
            {
                isInvincible = false;
                invincibleTimer = 0;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    public static void PlayerDamage(float damage)
    {
        if (!isInvincible)
        {
            UIManager.gageFillAmount -= damage;
            isInvincible = true;
        }
        
    }
}
