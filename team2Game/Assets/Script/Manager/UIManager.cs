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
    public static float hpGageFillAmount;               //星の温度ゲージの数値
    [SerializeField]
    private float hpGageStopTime = 3;                //ゲージが減少するまでの時間
    public static float hpGageStopTimer;                    //ゲージが減少するまでのタイマー
    public static bool isCombo;
    [SerializeField]
    private float comboGageStopTime = 3;                //ゲージが減少するまでの時間
    public static float comboGageStopTimer;                    //ゲージが減少するまでのタイマー

    public static bool comboGageStop;
    public static bool hpGageStop;

    [SerializeField]
    private Text wishUI;
    
    public static string wishText;

    [SerializeField]
    private Text answerUI;
    public static string answerText;

    [SerializeField]
    private Text debugUI;
    public static string debugtext;

    // Start is called before the first frame update
    void Start()
    {

        comboUI.enabled = false;
        comboTimerGage = comboTimerGageUI.GetComponent<Slider>();
        comboTimerGageUI.SetActive(false);

        hpGageFillAmount = 100;
        isCombo = false;
        hpGageStopTimer = 0;
        comboGageStopTimer = 0;

        wishUI.enabled = false;
        answerUI.enabled = false;

        wishText = "〇〇〇";

        answerText = "〇〇〇";

        comboGageStop = false;
        hpGageStop = false;
    }

    // Update is called once per frame
    void Update()
    {
            ComboUI();
            HPGageUI();
        
        WishUI();
        debugUI.text = "FPS;" + FPS.fps
            + "\n" + "isWishNow:" + WishManager.isWishNow
            + "\n" + "WishModeStay:" + PlayerManager.isWishStay
            + "\n" + "WishModeMode:" + PlayerManager.isWishMode
            + "\n" + "HpGageStop:" + hpGageStop
            + "\n" + "ComboGageStop:" + comboGageStop
            + "\n" + "isCombo:" + isCombo
            + "\n" + "isInvincible:" + PlayerManager.isInvincible
            + "\n" + "isStop:" + PlayerManager.isStop
            + "\n" + "isGameStop:" + GameManager.isGameStop
            + "\n" + debugtext;
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

            if (!comboGageStop && !GameManager.isGameStop)
            {
                comboGageStopTimer += Time.deltaTime;
                comboTimerGage.value = comboGageStopTime - comboGageStopTimer;
            }
            

            if(comboGageStopTimer >= comboGageStopTime && !PlayerManager.isWishMode)
            {
                if (isCombo)
                {
                    isCombo = false;
                    GameManager.combo = 0;
                }
                comboGageStopTimer = 0;
            }
        }
        else
        {
            if (comboUI.enabled)
            {
                comboUI.enabled = false;
                comboTimerGageUI.SetActive(false);
                comboTimerGage.value = hpGageStopTime;
            }
        }
    }

    private void HPGageUI()
    {
        gage.value = hpGageFillAmount;                //ゲージの数値を挿入

        if (!hpGageStop && !GameManager.isGameStop)
        {
            hpGageStopTimer += Time.deltaTime;
        }
        

        if (hpGageStopTimer >= hpGageStopTime && !PlayerManager.isWishMode)
        {
            if (hpGageFillAmount >= 0)
            {
                hpGageFillAmount -= 5 * Time.deltaTime;   //毎秒5ずつ減っていく
            }
            else if(hpGageFillAmount < 0)
            {
                GameManager.isOver = true;
            }
        }
        
        if(hpGageFillAmount >= 80)
        {
            gage.targetGraphic.color = Color.blue;
        }
        else
        {
            gage.targetGraphic.color = Color.red;
        }
    }

    private void WishUI()
    {
        if (PlayerManager.isWishMode)
        {
            wishUI.enabled = true;
            wishUI.text = wishText;
            answerUI.enabled = true;
            answerUI.text = answerText;
        }
        else
        {
            wishUI.enabled = false;
            answerUI.enabled = false;
        }
    }
}
