using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int combo;            //コンボ数
    public static int maxCombo;
    public static GameObject player;    //Player
    public static GameObject star;      //Star

    public static bool isClear;         //ゲームクリアしたか
    public static bool isOver;          //ゲームオーバーか

    public static bool isGameStop;     //ゲームの動きを止める

    //[SerializeField]
    //private bool DEBUG = false;
    public static bool debug;

    public static string sceneName;

    
    void Awake()
    {
        //Player取得
        player = GameObject.FindGameObjectWithTag("Player");
        //Star取得
        star = GameObject.FindGameObjectWithTag("Star");

        debug = false;
    }

    void Start()
    {
        //コンボ数を0に
        combo = 0;
        maxCombo = 0;

        //ゲームクリア・ゲームオーバー初期化
        isClear = false;
        isOver = false;

        isGameStop = false;

        sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        //エスケープキーが押されたらゲーム終了
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        
        //0が押されたら体力MAX
        if (Input.GetKeyDown(KeyCode.Alpha0) && debug)
        {
            UIManager.hpGageFillAmount = 100;
        }

        ////ゲームオーバーであればシーンをロードする
        //if (isOver &&!debug)
        //{
        //    SceneManager.LoadScene("GameOver");
        //}

        if (isClear)
        {
            SceneManager.LoadScene("GameClear");
        }

        if(Input.GetKeyDown(KeyCode.T) && Input.GetKey(KeyCode.Y))
        {
            debug = !debug;
        }

        if (maxCombo < combo)
        {
            maxCombo = combo;
            if(maxCombo > 99999)
            {
                maxCombo = 99999;
            }
        }
    }
}
