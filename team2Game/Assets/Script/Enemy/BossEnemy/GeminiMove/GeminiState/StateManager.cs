using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    //モーション管理用ディクショナリ
    private Dictionary<Move, IMove> states = new Dictionary<Move, IMove>();
    //現在のモーション
    private IMove currentMove = null;

    Helth helth;　　　　　　　　　//生存確認

    private void Awake()
    {
        helth = GetComponent<Helth>();
    }

    /// <summary>
    /// モーションの追加
    /// </summary>
    /// <param name="name"></param>
    /// <param name="move"></param>
    public void Add(Move name,IMove move)
    {
        if (states.ContainsKey(name))
        {
            return;
        }
        states.Add(name, move);
    }

    public void Change(Move name)
    {
        if(currentMove !=null)
        {
            currentMove.IsEnd();
        }
        currentMove = states[name];
        currentMove.Initialize();
    }

    // Update is called once per frame
    public void Update()
    {
        if(helth.Dead)
        {
            return;
        }
        if(currentMove == null)
        {
            return;
        }
        currentMove.Update();
        if(currentMove.IsEnd())
        {
            Change(currentMove.Next());
        }
    }
    /// <summary>
    /// 現在のモーション
    /// </summary>
    /// <returns></returns>
    public IMove CurrentMove()
    {
        return currentMove;
    }


}
