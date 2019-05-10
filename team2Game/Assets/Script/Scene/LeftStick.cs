using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftStick : Singleton<LeftStick>
{
    private enum Direction
    {
        TOP = 0,
        BOTTOM,
        LEFT,
        RIGHT,
        SIZE
    }

    //入力を受け付ける最低量(インスペクターから要調整)
    [SerializeField]
    private float minInputValue = 0.2f;

    //前フレーム押されていたかどうか
    private bool[] isPrevious;
    private bool[] isCurrent;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        //サイズ確保
        isPrevious = new bool[(int)Direction.SIZE];
        isCurrent = new bool[(int)Direction.SIZE];

        //値を全てfalseで初期化
        SetAllBool(isPrevious, false);
        SetAllBool(isCurrent, false);
    }

    void Update()
    {
        PreviousUpdate();
        CurrentUpdate();
    }

    private void SetAllBool(bool[] isArray, bool value)
    {
        for (int i = 0; i < isArray.Length; i++)
        {
            isArray[i] = value;
        }
    }

    //previousの情報を更新
    private void PreviousUpdate()
    {
        for (int i = 0; i < isPrevious.Length; i++)
        {
            isPrevious[i] = isCurrent[i];
        }
    }

    //currentの情報を更新
    private void CurrentUpdate()
    {
        isCurrent[(int)Direction.TOP] = IsTop();
        isCurrent[(int)Direction.BOTTOM] = IsBottom();
        isCurrent[(int)Direction.RIGHT] = IsRight();
        isCurrent[(int)Direction.LEFT] = IsLeft();
    }

    //横方向の入力量を返す
    public float Horizontal()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    //縦方向の入力量を返す
    public float Vertical()
    {
        return Input.GetAxisRaw("Vertical");
    }

    #region 押されている間ずっとtrue

    //上
    public bool IsTop()
    {
        return Vertical() > minInputValue;
    }
    //下
    public bool IsBottom()
    {
        return Vertical() < -minInputValue;
    }
    //左
    public bool IsLeft()
    {
        return Horizontal() < -minInputValue;
    }
    //右
    public bool IsRight()
    {
        return Horizontal() > minInputValue;
    }

    #endregion

    #region 押された瞬間だけtrue

    //上
    public bool IsTopDown()
    {
        return !isPrevious[(int)Direction.TOP] && isCurrent[(int)Direction.TOP];
    }
    //下
    public bool IsBottomDown()
    {
        return !isPrevious[(int)Direction.BOTTOM] && isCurrent[(int)Direction.BOTTOM];
    }
    //左
    public bool IsLeftDown()
    {
        return !isPrevious[(int)Direction.LEFT] && isCurrent[(int)Direction.LEFT];
    }
    //右
    public bool IsRightDown()
    {
        return !isPrevious[(int)Direction.RIGHT] && isCurrent[(int)Direction.RIGHT];
    }

    #endregion

    #region 離された瞬間だけtrue

    //上
    public bool IsTopUp()
    {
        return isPrevious[(int)Direction.TOP] && !isCurrent[(int)Direction.TOP];
    }
    //下
    public bool IsBottomUp()
    {
        return isPrevious[(int)Direction.BOTTOM] && !isCurrent[(int)Direction.BOTTOM];
    }
    //左
    public bool IsLeftUp()
    {
        return isPrevious[(int)Direction.LEFT] && !isCurrent[(int)Direction.LEFT];
    }
    //右
    public bool IsRightUp()
    {
        return isPrevious[(int)Direction.RIGHT] && !isCurrent[(int)Direction.RIGHT];
    }

    #endregion

    //横入力されているか
    public bool IsHorizontal()
    {
        return IsLeft() || IsRight();
    }
    //縦入力されているか
    public bool IsVertical()
    {
        return IsTop() || IsBottom();
    }
}
