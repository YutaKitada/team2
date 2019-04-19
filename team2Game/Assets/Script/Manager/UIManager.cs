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


    public static string wishText;

    public static string answerText;

    [SerializeField]
    private List<Sprite> buttonSprites;

    [SerializeField]
    private Text debugUI;
    public static string debugtext;

    [SerializeField]
    private int maxWishButton = 7;
    private int wishButtonArrayNum;
    [SerializeField]
    private GameObject buttonObject;
    [SerializeField]
    private GameObject mistakeObject;
    private List<GameObject> answerButtonList;
    private List<GameObject> wishButtonList;
    [SerializeField]
    private GameObject buttonParent;


    [SerializeField]
    private Slider wishTimer;
    public static float wishTimerFillamount = -3;

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

        wishText = "";

        answerText = "";
        

        comboGageStop = false;
        hpGageStop = false;

        wishButtonArrayNum = maxWishButton * 2 - 1;

        float buttonPositionX = -30 * maxWishButton;

        answerButtonList = new List<GameObject>();
        wishButtonList = new List<GameObject>();

        for (int i = 0;i < wishButtonArrayNum; i++)
        {
            answerButtonList.Add(Instantiate(buttonObject, buttonParent.transform));
            wishButtonList.Add(Instantiate(buttonObject, buttonParent.transform));
            answerButtonList[i].transform.localPosition = new Vector3(buttonPositionX, 0);
            wishButtonList[i].transform.localPosition = new Vector3(buttonPositionX, -60);
            buttonPositionX += 30;
        }
        wishTimer.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        AnswerUI();
        WishUI();
        ComboUI();
        HPGageUI();
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


            if (comboGageStopTimer >= comboGageStopTime && !PlayerManager.isWishMode)
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
            else if (hpGageFillAmount < 0)
            {
                GameManager.isOver = true;
            }
        }
    }

    private void AnswerUI()
    {
        if (PlayerManager.isWishMode)
        {
            wishTimer.gameObject.SetActive(true);
            wishTimer.value = wishTimerFillamount;

            int wishButtonNumber = answerText.Length;

            bool space = false;

            int answerTextNumber = 0;

            for (int i = maxWishButton - wishButtonNumber; i < answerText.Length + 6; i++)
            {
                if (space)
                {
                    space = false;
                }
                else if (!space)
                {
                    switch (answerText.Substring(answerTextNumber, 1))
                    {
                        case "A":
                            answerButtonList[i].GetComponent<Image>().sprite = buttonSprites[0];
                            break;
                        case "B":
                            answerButtonList[i].GetComponent<Image>().sprite = buttonSprites[1];
                            break;
                        case "X":
                            answerButtonList[i].GetComponent<Image>().sprite = buttonSprites[2];
                            break;
                        case "Y":
                            answerButtonList[i].GetComponent<Image>().sprite = buttonSprites[3];
                            break;
                    }
                    answerTextNumber++;
                    space = true;
                }
            }
        }
        else
        {
            wishTimer.gameObject.SetActive(false);
            wishTimerFillamount = -3;
            for (int i = 0; i < answerButtonList.Count; i++)
            {
                answerButtonList[i].GetComponent<Image>().sprite = buttonSprites[4];
            }
        }
    }

    private void WishUI()
    {
        if (PlayerManager.isWishMode)
        {
            int wishButtonNumber = answerText.Length;

            bool space = false;

            int wishTextNumber = 0;

            for (int i = maxWishButton - wishButtonNumber; i < answerText.Length + 6; i++)
            {
                if (space)
                {
                    space = false;
                }
                else if (!space)
                {
                    if(wishText.Length-1 >= wishTextNumber)
                    {
                        switch (wishText.Substring(wishTextNumber, 1))
                        {
                            case "A":
                                wishButtonList[i].GetComponent<Image>().sprite = buttonSprites[0];
                                break;
                            case "B":
                                wishButtonList[i].GetComponent<Image>().sprite = buttonSprites[1];
                                break;
                            case "X":
                                wishButtonList[i].GetComponent<Image>().sprite = buttonSprites[2];
                                break;
                            case "Y":
                                wishButtonList[i].GetComponent<Image>().sprite = buttonSprites[3];
                                break;
                        }
                        if(wishButtonList[i].GetComponent<Image>().sprite != answerButtonList[i].GetComponent<Image>().sprite)
                        {
                            mistakeObject.SetActive(true);
                            mistakeObject.transform.position = wishButtonList[i].transform.position;
                        }
                        wishTextNumber++;
                        space = true;
                    }
                    
                }
            }
        }
        else
        {
            for (int i = 0; i < answerButtonList.Count; i++)
            {
                wishButtonList[i].GetComponent<Image>().sprite = buttonSprites[4];

                mistakeObject.SetActive(false);
            }
        }
    }
}
