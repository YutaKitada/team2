using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int combo;            //コンボ数
    public static GameObject player;    //Player
    public static GameObject star;      //Star

    public static bool isClear;         //ゲームクリアしたか
    public static bool isOver;          //ゲームオーバーか
    
    void Awake()
    {
        //コンボ数を0に
        combo = 0;
        //Player取得
        player = GameObject.FindGameObjectWithTag("Player");
        //Star取得
        star = GameObject.FindGameObjectWithTag("Star");

        //ゲームクリア・ゲームオーバー初期化
        isClear = false;
        isOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        //エスケープキーが押されたらゲーム終了
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //BGM確認
        if (!SoundManager.CheckBGM(1))
        {
            SoundManager.PlayBGM(1);
        }

        //0が押されたら体力MAX
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            UIManager.gageFillAmount = 100;
        }

        //ゲームオーバーであればシーンをロードする
        if (isOver)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
