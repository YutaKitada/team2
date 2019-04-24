using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WishManager : MonoBehaviour
{

    private string wishCommand;

    private bool isWishMode;
    private bool isWish;

    private bool stickLock;

    public static GameObject player;            //Playerオブジェクト
    private MeshRenderer playerRender;
    public static GameObject star;              //Starオブジェクト

    public static bool isEverCombo;

    public static bool isWishNow;               //現在願い事効果中か否か
    private float wishTime;
    private float wishTimer;
    private int wishNumber;
    private bool commandSuccess;

    [SerializeField]
    private TextAsset csvFile;
    private List<string[]> wishDatas = new List<string[]>();
    
    [SerializeField]
    private int wishNumber_Debug = 1;

    // Start is called before the first frame update
    void Start()
    {

        wishCommand = "";

        isWish = false;
        isWishMode = false;
        isEverCombo = false;

        commandSuccess = false;

        stickLock = false;

        //Player取得
        player = GameObject.FindGameObjectWithTag("Player");
        playerRender = player.GetComponent<MeshRenderer>();
        //Star取得
        star = GameObject.FindGameObjectWithTag("Star");

        isWishNow = false;

        //var csvFile = Resources.Load("testCSV") as TextAsset; // Resouces下のCSV読み込み

        //Debug.Log(csvFile.text);


        // csvファイルの内容をStringReaderに変換
        var reader = new StringReader(csvFile.text);

        // csvファイルの内容を一行ずつ末尾まで取得しリストを作成
        while (reader.Peek() > -1)
        {
            // 一行読み込む
            var lineData = reader.ReadLine();
            // カンマ(,)区切りのデータを文字列の配列に変換
            var address = lineData.Split(',');
            // リストに追加
            wishDatas.Add(address);
            // 末尾まで繰り返し...
        }

        // ログに読み込んだデータを表示する
        foreach (var data in wishDatas)
        {
            Debug.Log("DATA:" + data[0] + " / " + data[1] + " / " + data[2] + " / " + data[3] + " / " + data[4]);
        }
        //Debug.Log("DATA:" + addressDatas[0] + " / " + addressDatas[1] + " / " + addressDatas[2]);

        wishTime = 0;
        wishTimer = 0;
        wishNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        WishText();

        //if (PlayerManager.isWishStay)
        //{
        //    playerRender.material.color = Color.blue;
        //}
        //else if (!PlayerManager.isWishStay)
        //{
        //    playerRender.material.color = Color.red;
        //}
    }

    private void WishText()
    {
        if (PlayerManager.isWishMode)
        {
            if (!isWishMode)
            {
                isWishMode = true;
                GameManager.isGameStop = true;
                if (GameManager.debug)
                {
                    UIManager.answerText = wishDatas[wishNumber_Debug][1];
                }
                else
                {
                    UIManager.answerText = wishDatas[0][1];
                }
                
            }
            else
            {
                if (!isWish)
                {
                    if (wishCommand != "")
                    {
                        if (wishCommand.Substring(wishCommand.Length - 1, 1) != UIManager.answerText.Substring(wishCommand.Length - 1, 1))
                        {
                            UIManager.wishText = "だめです";
                            isWish = true;
                            SoundManager.PlaySE(3);
                            PlayerManager.PlayerDamage(30);
                        }
                    }
                    Wish(wishCommand);

                    if (Input.GetButtonDown("AButton"))
                    {
                        wishCommand += "A";
                        SoundManager.PlaySE(1);
                    }
                    else if (Input.GetButtonDown("BButton"))
                    {
                        wishCommand += "B";
                        SoundManager.PlaySE(1);
                    }
                    else if (Input.GetButtonDown("XButton"))
                    {
                        wishCommand += "X";
                        SoundManager.PlaySE(1);
                    }
                    else if (Input.GetButtonDown("YButton"))
                    {
                        wishCommand += "Y";
                        SoundManager.PlaySE(1);
                    }
                    //else if (Input.GetAxisRaw("Horizontal") >= 1 &&!stickLock)
                    //{
                    //    wishCommand += "→";
                    //    stickLock = true;
                    //}
                    //else if (Input.GetAxisRaw("Horizontal") <= -1 && !stickLock)
                    //{
                    //    wishCommand += "←";
                    //    stickLock = true;
                    //}
                    //else if (Input.GetAxisRaw("Vertical") <= -1 && !stickLock)
                    //{
                    //    wishCommand += "↑";
                    //    stickLock = true;
                    //}
                    //else if (Input.GetAxisRaw("Vertical") >= 1 && !stickLock)
                    //{
                    //    wishCommand += "↓";
                    //    stickLock = true;
                    //}
                    //else if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
                    //{
                    //    stickLock = false;
                    //}
                    UIManager.wishText = wishCommand;
                    
                }

            }
        }
        else
        {
            isWish = false;
            wishCommand = "";
            UIManager.wishText = wishCommand;
            isWishMode = false;
            stickLock = false;
            GameManager.isGameStop = false;
            if (commandSuccess)
            {
                commandSuccess = false;
                isWishNow = true;
            }
        }

        if (isWishNow)
        {
            wishTimer += Time.deltaTime;
            Wish(wishNumber);
            if (wishTimer >= wishTime)
            {
                isWishNow = false;
                wishNumber = 0;
                WishEnd();
            }
        }
    }


    private void Wish(string command)
    {
        for (int i = 0; i < wishDatas.Count; i++)
        {
            if (wishDatas[i][1] == command)
            {
                wishTime = float.Parse(wishDatas[i][2]);
                PlayerManager.PlayerDamage(float.Parse(wishDatas[i][3]));
                isWish = true;
                commandSuccess = true;
                wishTimer = 0;
                wishCommand = wishDatas[i][4];
                UIManager.wishText = wishCommand;
                wishNumber = i;
                SoundManager.PlaySE(10);
            }
        }
    }

    private void Wish(int wishNum)
    {
        switch (wishNum)
        {
            case 1:
                ReturnStar();
                break;
            case 2:
                StopCombo();
                EternalCombo();
                break;
            case 3:
                EverCombo();
                break;
        }
    }

    private void WishEnd()
    {
        UIManager.comboGageStop = false;
        isEverCombo = false;
        ComboUI.comboTimerStop = false;
    }

    private void ReturnStar()
    {
        if (!PlayerManager.haveStar && star.GetComponent<StarMovement>().returnPlayer)
        {
            PlayerManager.haveStar = true;
            star.GetComponent<StarMovement>().returnPlayer = false;
        }
    }

    private void StopCombo()
    {
        ComboUI.comboTimerStop = true;
    }

    private void EverCombo()
    {
        isEverCombo = true;
    }

    private void EternalCombo()
    {
        GameManager.combo++;
    }
}
