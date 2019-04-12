using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static int DefeatedCount//倒した敵のカウントプロパティ
    {
        get;
        set;
    }
    
    void Awake()
    {
        DefeatedCount = 0;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("倒した数:" + DefeatedCount);
        }
    }
}
