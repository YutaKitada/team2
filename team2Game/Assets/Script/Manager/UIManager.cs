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
    
    public static string wishText;

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
        
        wishText = "〇〇〇";
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
            wishUI.enabled = true;
            wishUI.text = wishText;
        }
        else
        {
            wishUI.enabled = false;
        }
    }
}
