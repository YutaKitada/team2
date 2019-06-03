using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// モーション
/// </summary>
public enum Move
{
    None,                 //立ち
    ThrowAttack,   　     //雪投げ
    RollBallAttack,       //岩転がし
    SnowStorm,            //吹雪
    HitAttack,            //殴る
}

public interface IMove
{
    void Initialize();   //初期化
    void Update();       //更新
    bool IsEnd();　　　　//状態の終了
    Move Next();         //次のモーション
}
