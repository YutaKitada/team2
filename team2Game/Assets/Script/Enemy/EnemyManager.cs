using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemyの数取得クラス
/// </summary>
public class EnemyManager : MonoBehaviour
{
    public static int DefeatedCount//倒した敵のカウントプロパティ
    {
        get;
        set;
    }

    GameObject[] enemys;//Enemy用の配列
    public static int EnemyCount//Enemyの総数
    {
        get;
        private set;
    }
    void Awake()
    {
        //初期化
        DefeatedCount = 0;
        for(int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i] != null)
            {
                enemys[i] = null;
            }
        }
    }

    void Start()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");//シーン上の全てのEnemyを取得
        EnemyCount = enemys.Length;//Enemyの総数を取得
        Debug.Log("敵の総数："+EnemyCount);
    }
}
