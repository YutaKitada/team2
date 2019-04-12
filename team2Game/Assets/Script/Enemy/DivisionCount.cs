using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 分裂した敵（子オブジェクト）を倒したときのカウントをするクラス
/// </summary>
public class DivisionCount : MonoBehaviour
{
    bool isCount = false;

    void FixedUpdate()
    {
        //1度、カウントをしたら処理を起こさない
        if (isCount) return;

        //分裂した敵（子オブジェクト）を全て倒したらカウント
        if (transform.childCount == 0)
        {
            isCount = true;
            EnemyManager.DefeatedCount++;
            Debug.Log(EnemyManager.DefeatedCount);
        }
    }
}
