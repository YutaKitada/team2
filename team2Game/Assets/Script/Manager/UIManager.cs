using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text comboUI;
    [SerializeField]
    private GameObject comboTimerGageUI;
    private Slider comboTimerGage;

    [SerializeField]
    private Slider gage;                            //星の温度ゲージ
    public static float gageFillAmount;               //星の温度ゲージの数値
    [SerializeField]
    private float gageStopTime = 3;                //ゲージが減少するまでの時間
    public static float gageStopTimer;                    //ゲージが減少するまでのタイマー
    public static bool isCombo;

    [SerializeField]
    private Text wishUI;
    private string wish_one, wish_two, wish_three;
    private bool isWish;

    // Start is called before the first frame update
    void Start()
    {

        comboUI.enabled = false;
        comboTimerGage = comboTimerGageUI.GetComponent<Slider>();
        comboTimerGageUI.SetActive(false);

        gageFillAmount = 100;
        isCombo = false;
        gageStopTimer = 0;

        wishUI.enabled = false;
        wish_one = "〇";
        wish_two = "〇";
        wish_three = "〇";
        isWish = false;
    }

    // Update is called once per frame
    void Update()
    {
        ComboUI();
        HPGageUI();
        WishUI();
    }

    private void ComboUI()
    {
        if (GameManager.combo >= 2)
        {
            if (!comboUI.enabled)
            {
                comboUI.enabled = true;
                comboTimerGageUI.SetActive(true);
            }
            comboUI.text = GameManager.combo + "コンボ";
            comboTimerGage.value = gageStopTime - gageStopTimer;
        }
        else
        {
            if (comboUI.enabled)
            {
                comboUI.enabled = false;
                comboTimerGageUI.SetActive(false);
                comboTimerGage.value = gageStopTime;
            }
        }
    }

    private void HPGageUI()
    {
        gage.value = gageFillAmount;                //ゲージの数値を挿入

        gageStopTimer += Time.deltaTime;

        if (gageStopTimer >= gageStopTime)
        {
            if (gageFillAmount >= 0)
            {
                gageFillAmount -= 5 * Time.deltaTime;   //毎秒5ずつ減っていく
            }
            else if(gageFillAmount < 0)
            {
                GameManager.isOver = true;
            }
            if (isCombo)
            {
                isCombo = false;
                GameManager.combo = 0;
            }
        }
    }

    private void WishUI()
    {
        if (PlayerManager.isWishMode)
        {
            if (!wishUI.enabled)
            {
                wishUI.enabled = true;
            }
            if (Input.GetButtonDown("AButton"))
            {
                if(wish_one == "〇")
                {
                    wish_one = "A";
                }
                else if (wish_two == "〇")
                {
                    wish_two = "A";
                }
                else if (wish_three == "〇")
                {
                    wish_three = "A";
                }
            }
            else if(Input.GetButtonDown("BButton"))
            {
                if (wish_one == "〇")
                {
                    wish_one = "B";
                }
                else if (wish_two == "〇")
                {
                    wish_two = "B";
                }
                else if (wish_three == "〇")
                {
                    wish_three = "B";
                }
            }
            else if (Input.GetButtonDown("XButton"))
            {
                if (wish_one == "〇")
                {
                    wish_one = "X";
                }
                else if (wish_two == "〇")
                {
                    wish_two = "X";
                }
                else if (wish_three == "〇")
                {
                    wish_three = "X";
                }
            }
            else if (Input.GetButtonDown("YButton"))
            {
                if (wish_one == "〇")
                {
                    wish_one = "Y";
                }
                else if (wish_two == "〇")
                {
                    wish_two = "Y";
                }
                else if (wish_three == "〇")
                {
                    wish_three = "Y";
                }
            }

            wishUI.text = wish_one + "　" + wish_two + "　" + wish_three;

            if (!PlayerManager.isStop &&!isWish && wish_one != "〇"&& wish_two != "〇" && wish_three != "〇")
            {
                WishManager.Wish(wishUI.text);
                isWish = true;
            }
        }
        else
        {
            wishUI.enabled = false;
            isWish = false;
            wish_one = "〇";
            wish_two = "〇";
            wish_three = "〇";
        }
    }
}
