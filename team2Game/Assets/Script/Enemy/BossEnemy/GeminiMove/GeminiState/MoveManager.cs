using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{

    private StateManager stateManager;   //シーン管理者

    // Start is called before the first frame update
    void Start()
    {
        //状態を追加
        stateManager = GetComponent<StateManager>();
        stateManager.Add(Move.None, GetComponent<NoneState>());
        stateManager.Add(Move.ThrowAttack, GetComponent<ThrowAttack>());
        stateManager.Add(Move.RollBallAttack, GetComponent<RollBallAttack>());
        stateManager.Add(Move.SnowStorm, GetComponent<SnowStorm>());
        stateManager.Add(Move.HitAttack, GetComponent<HitAttack>());
        stateManager.Change(Move.None);                   //最初はNone

    }

    // Update is called once per frame
    void Update()
    {
        stateManager.Update();
    }
}
