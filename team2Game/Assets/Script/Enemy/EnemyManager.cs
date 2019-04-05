using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static int DefeatedCount//倒された敵のカウントプロパティ
    {
        get;
        set;
    }
    
    void Awake()
    {
        DefeatedCount = 0;
    }
}
