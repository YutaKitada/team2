﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 子にあるオブジェクトを円状に配置するクラス
/// </summary>
public class CircleDeployer : MonoBehaviour
{
    //半径
    [SerializeField]
    private float radius;


    private void Awake()
    {
        Deploy();
    }
    //Inspectorの内容(半径)が変更された時に実行
    private void OnValidate()
    {
        Deploy();
    }

    //子を円状に配置する(ContextMenuで鍵マークの所にメニュー追加)
    [ContextMenu("Deploy")]
    private void Deploy()
    {
        //子を取得
        List<GameObject> childList = new List<GameObject>();
        foreach(Transform child in transform)
        {
            childList.Add(child.gameObject);
        }
        //数値、アルファベット順に並べ替え
        childList.Sort((a, b) =>
        {
            return string.Compare(a.name, b.name);
        });

        //オブジェクト間の角度差
        float angleDiff = 360f / (float)childList.Count;

        //各オブジェクトを円状に配置
        for (int i = 0; i < childList.Count; i++)
        {
            Vector3 childPostion = transform.position;

            float angle = (180 - angleDiff * i) * Mathf.Deg2Rad;
            childPostion.z += radius * Mathf.Cos(angle);
            childPostion.y += radius * Mathf.Sin(angle);

            childList[i].transform.position = childPostion;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
