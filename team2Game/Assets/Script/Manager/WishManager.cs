using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WishManager : MonoBehaviour
{

    private string wishCommand;                 //入力しているコマンド

    private bool commandInput;                  //コマンド入力中か否か
    private bool commandRestriction;            //コマンドが入力できるか否か

    public static GameObject player;            //Playerオブジェクト
    public static GameObject star;              //Starオブジェクト

    public static bool isEverCombo;             //無限にコンボをつなげられるか否か

    public static bool isWishNow;               //現在願い事効果中か否か


    private float wishTime;                     //願い事の効果時間
    private float wishTimer;                    //願い事の効果時間用タイマー
    private int wishNumber;                     //願い事の番号

    private bool commandSuccess;                //コマンド入力が成功したか否か

    [SerializeField]
    private TextAsset csvFile;
    private List<string[]> wishDatas = new List<string[]>();
    
    [SerializeField]
    private int wishNumber_Debug = 1;
    private int wishNum;

    [SerializeField]
    private GameObject wishStar;
    private float showerTimer;

    public static bool isMeteorShower;
    public static bool isTackleStar;
    public static bool isChase;

    public static int wishUseNumber;

    // Start is called before the first frame update
    void Start()
    {

        wishCommand = "";

        commandRestriction = false;
        commandInput = false;
        isEverCombo = false;

        commandSuccess = false;

        //Player取得
        player = GameObject.FindGameObjectWithTag("Player");
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

        showerTimer = 0;

        wishNum = 0;

        isMeteorShower = false;
        isTackleStar = false;
        isChase = false;

        wishUseNumber = 0;
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
            if (!commandInput)
            {
                commandInput = true;
                GameManager.isGameStop = true;
                if (GameManager.debug)
                {
                    UIManager.answerText = wishDatas[wishNumber_Debug][1];
                }
                else
                {
                    wishNum = Random.Range(1, 4);
                    UIManager.answerText = wishDatas[wishNum][1];
                }
                
            }
            else
            {
                if (!commandRestriction)
                {
                    if (wishCommand != "")
                    {
                        if (wishCommand.Substring(wishCommand.Length - 1, 1) != UIManager.answerText.Substring(wishCommand.Length - 1, 1))
                        {
                            UIManager.wishText = "だめです";
                            commandRestriction = true;
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
                    UIManager.wishText = wishCommand;
                    
                }

            }
        }
        else
        {
            commandRestriction = false;
            wishCommand = "";
            UIManager.wishText = wishCommand;
            commandInput = false;
            GameManager.isGameStop = false;
            if (commandSuccess)
            {
                commandSuccess = false;
                isWishNow = true;
                wishUseNumber++;
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
                commandRestriction = true;
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
                EverCombo();
                break;
            case 3:
                Shower();
                break;
            case 4:
                Tackle();
                break;
            case 5:
                ChaseStar();
                break;
        }
    }

    private void WishEnd()
    {
        UIManager.comboGageStop = false;
        isEverCombo = false;
        ComboUI.comboTimerStop = false;
        showerTimer = 0;
        isMeteorShower = false;
        isTackleStar = false;
        isChase = false;
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

    private void Shower()
    {
        isMeteorShower = true;
        showerTimer -= Time.deltaTime;

        if(showerTimer < 0)
        {
            Instantiate(wishStar, player.transform.position + new Vector3(Random.Range(-20, 0), 20), Quaternion.identity);
            showerTimer = 0.5f;
        }
    }

    private void Tackle()
    {
        isTackleStar = true;
    }

    private void ChaseStar()
    {
        isChase = true;

        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bosss = GameObject.FindGameObjectsWithTag("BossEnemy");

        GameObject enemy = null;

        float distance = -1;

        if(enemys != null)
        {
            foreach (var n in enemys)
            {
                if (distance == -1)
                {
                    distance = Vector3.Distance(player.transform.position, n.transform.position);
                    enemy = n;
                }
                else if (distance >= Vector3.Distance(player.transform.position, n.transform.position))
                {
                    distance = Vector3.Distance(player.transform.position, n.transform.position);
                    enemy = n;
                }
            }
        }
        if(bosss != null)
        {
            foreach (var n in bosss)
            {
                if (distance == -1)
                {
                    distance = Vector3.Distance(player.transform.position, n.transform.position);
                    enemy = n;
                }
                else if (distance >= Vector3.Distance(player.transform.position, n.transform.position))
                {
                    distance = Vector3.Distance(player.transform.position, n.transform.position);
                    enemy = n;
                }
            }
        }
        

        if (!PlayerManager.haveStar && !star.GetComponent<StarMovement>().returnPlayer)
        {
            star.GetComponent<Rigidbody>().velocity = (enemy.transform.position + new Vector3(0,1) - star.transform.position).normalized * 30;
        }
    }
}
