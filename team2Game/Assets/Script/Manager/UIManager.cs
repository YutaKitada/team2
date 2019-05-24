using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private Image hpGage;                            //星の温度ゲージ
    public static float hpGageFillAmount;               //星の温度ゲージの数値
    [SerializeField]
    private float hpGageStopTime = 3;                //ゲージが減少するまでの時間
    public static float hpGageStopTimer;                    //ゲージが減少するまでのタイマー
    public static bool isCombo;
    [SerializeField]
    private float comboGageStopTime = 3;                //ゲージが減少するまでの時間
    public static float comboGageStopTimer;                    //ゲージが減少するまでのタイマー

    public static bool comboGageStop;                   //コンボゲージを止めるFlag
    public static bool hpGageStop;                      //HPゲージを止めるFlag
    
    public static string wishText;                      //願い事用テキスト

    public static string answerText;                    //願い事の答え用テキスト

    [SerializeField]
    private List<Sprite> buttonSprites;                 //ボタンの画像List

    [SerializeField]
    private Text debugUI;                               //デバッグのテキスト
    public static string debugtext;                     //外からデバッグのテキストをいじれるように

    [SerializeField]
    private int maxWishButton = 7;                      //願い事のコマンド数の上限
    private int wishButtonArrayNum;                     //コマンド配置用の数値
    [SerializeField]
    private GameObject mistakeObject;                   //コマンドをミスした時に表示するオブジェクト
    private List<GameObject> answerButtonList;          //願い事の答え用List
    private List<GameObject> wishButtonList;            //願い事のコマンド用List
    [SerializeField]
    private GameObject buttonParent;                    //コマンドボタンを配置する親
    [SerializeField]
    private GameObject buttonObject;                    //コマンドボタン用オブジェクト


    [SerializeField]
    private Slider wishTimer;                           //願い事コマンドの入力受付時間表示用スライダー
    public static float wishTimerFillamount = -3;       //願い事コマンドの入力受付時間

    [SerializeField]
    private Image wishYButton;                          //コマンド入力可能時に表示するYボタン

    [SerializeField]
    private GameObject circleObject;                   //コマンドをミスした時に表示するオブジェクト
    private List<GameObject> circleList;

    [SerializeField]
    private Image fade;

    // Start is called before the first frame update
    void Start()
    {
        //HPの初期値を100に
        hpGageFillAmount = 100;
        //コンボ中ではない
        isCombo = false;
        //各ゲージを止めるタイマーを初期値に
        hpGageStopTimer = 0;
        comboGageStopTimer = 0;

        //各テキストを初期値に
        wishText = "";
        answerText = "";
        
        //各コンボゲージを止めるFlagを初期値に
        comboGageStop = false;
        hpGageStop = false;


        //願い事のボタン用のポジションを指定
        wishButtonArrayNum = maxWishButton * 2 - 1;
        float buttonPositionX = -30 * maxWishButton;
        answerButtonList = new List<GameObject>();
        wishButtonList = new List<GameObject>();
        for (int i = 0;i < wishButtonArrayNum; i++)
        {
            answerButtonList.Add(Instantiate(buttonObject, buttonParent.transform));
            wishButtonList.Add(Instantiate(buttonObject, buttonParent.transform));
            answerButtonList[i].transform.localPosition = new Vector3(buttonPositionX, 0);
            //wishButtonList[i].transform.localPosition = new Vector3(buttonPositionX, -60);
            wishButtonList[i].transform.localPosition = new Vector3(buttonPositionX, 0);
            buttonPositionX += 30;
        }

        //願い事入力受付時間用スライダーを非表示に
        wishTimer.gameObject.SetActive(false);

        //Yボタンを非表示に
        wishYButton.enabled = false;

        circleList = new List<GameObject>();

        
    }

    // Update is called once per frame
    void Update()
    {
        AnswerUI();
        WishUI();
        //ComboUI();
        HPGageUI();
        YButton();

        if (GameManager.debug)
        {
            debugUI.enabled = true;
        }
        else
        {
            debugUI.enabled = false;
        }

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
            + "\n" +  "isTackleStar:"+ WishManager.isTackleStar + debugtext;
    }

    //private void ComboUI()
    //{
    //    //2コンボ以上つながっていれば
    //    if (GameManager.combo >= 2)
    //    {

    //        if (!comboGageStop && !GameManager.isGameStop)
    //        {
    //            comboGageStopTimer += Time.deltaTime;
    //        }


    //        if (comboGageStopTimer >= comboGageStopTime && !PlayerManager.isWishMode)
    //        {
    //            if (isCombo)
    //            {
    //                isCombo = false;
    //                GameManager.combo = 0;
    //            }
    //            comboGageStopTimer = 0;
    //        }
    //    }
    //}

    private void HPGageUI()
    {
        hpGage.fillAmount = hpGageFillAmount/100;                //ゲージの数値を挿入

        //ゲージが止まっていない、もしくはゲームが止まっていない場合
        if (!hpGageStop && !GameManager.isGameStop)
        {
            //HPゲージの減少を止めるタイマーを作動
            hpGageStopTimer += Time.deltaTime;
        }

        //タイマーが規定値を超えた、もしくは願い事コマンド入力状態でなければ
        if (hpGageStopTimer >= hpGageStopTime && !PlayerManager.isWishMode)
        {
            //HPゲージが0以上であれば
            if (hpGageFillAmount > 0)
            {
                hpGageFillAmount -= 5 * Time.deltaTime;   //毎秒5ずつ減っていく
            }
            
        }
        //HPゲージが0未満になったらゲームオーバーに
        if (hpGageFillAmount <= 0)
        {
            GameManager.isOver = true;
        }

        if(hpGageFillAmount <= 20)
        {
            fade.color = new Color(0, 0, 0, (20 - hpGageFillAmount) / 20);
        }
        else
        {
            fade.color = new Color(0, 0, 0, 0);
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
                        if (wishButtonList[i].GetComponent<Image>().sprite != answerButtonList[i].GetComponent<Image>().sprite)
                        {
                            mistakeObject.SetActive(true);
                            mistakeObject.transform.position = wishButtonList[i].transform.position;
                        }
                        else
                        {
                            circleList.Add(Instantiate(circleObject, wishButtonList[i].transform.position, Quaternion.identity, buttonParent.transform));
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
            foreach(var n in circleList)
            {
                Destroy(n.gameObject);
            }
        }
    }

    private void YButton()
    {
        if(hpGageFillAmount >= 75 && !WishManager.isWishNow)
        {
            wishYButton.enabled = true;
        }
        else
        {
            wishYButton.enabled = false;
        }
    }
}
