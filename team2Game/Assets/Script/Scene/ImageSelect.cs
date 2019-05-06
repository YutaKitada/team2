using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ImageSelect : MonoBehaviour
{
    private List<List<GameObject>> stageList;

    public static int storageGroup = 0;
    public static int storageStage = 0;

    //現在選択中のstageの添え字
    private int currentStageIndex;
    //現在選択中のGroupの添え字
    private int currentGroupIndex;
    //現在選択中のステージの名前
    public static string currentStageName;

    //最端かどうか
    private bool isTop;
    private bool isBottom;
    private bool isLeft;
    private bool isRight;


    void Start()
    {
        //stageList[0][0]が最初に選んでいるものとする
        currentGroupIndex = storageGroup;
        currentStageIndex = storageStage;


        StageListInit();
    }

    private void StageListInit()
    {
        stageList = new List<List<GameObject>>();

        //各グループの中にあるステージ数分だけ要素数を確保 -1しているのはTextがあるから
        for (int i = 0; i < transform.childCount; i++)
        {
            //子オブジェクト(Group)のi番目の、子オブジェクト(ImageUI)をリストにして登録
            stageList.Add(GetChilds(transform.GetChild(i).gameObject));
        }
    }

    //引数の子オブジェクトをリストに格納して返す
    private List<GameObject> GetChilds(GameObject parent)
    {
        List<GameObject> result = new List<GameObject>();

        foreach (Transform child in parent.transform)
        {
            result.Add(child.gameObject);
        }

        return result;
    }

    void Update()
    {
        storageGroup = currentGroupIndex;
        storageStage = currentStageIndex;

        Clamp();

        MoveHorizon();
        MoveVertical();

        TestFunc1();
        TestFunc2();
    }

    //選択の横移動
    private void MoveHorizon()
    {
        if (LeftStick.Instance.IsRightDown() && !isRight)
        {
            isLeft = false;
            //選択しているStageを変更する
            currentStageIndex++;
        }
        if (LeftStick.Instance.IsLeftDown() && !isLeft)
        {
            isRight = false;
            //選択しているStageを変更する
            currentStageIndex--;
        }
    }

    //選択の縦移動
    private void MoveVertical()
    {
        if (LeftStick.Instance.IsTopDown() && !isTop)
        {
            isBottom = false;
            //選択しているGroupを変更する
            currentGroupIndex++;
        }
        if (LeftStick.Instance.IsBottomDown() && !isBottom)
        {
            isTop = false;
            //選択しているGroupを変更する
            currentGroupIndex--;
        }
    }

    //全てのステージの色を白にする
    private void TestFunc1()
    {
        foreach (var group in stageList)
        {
            foreach (var stage in group)
            {
                Image image = stage.GetComponent<Image>();
                image.color = Color.white;
            }
        }
    }

    //現在選択中のステージの色を変える
    private void TestFunc2()
    {
        Image image = stageList[currentGroupIndex][currentStageIndex].GetComponent<Image>();
        image.color = Color.red;
        currentStageName = image.transform.GetChild(0).gameObject.name;

    }

    //選択範囲を決める
    private void Clamp()
    {

        //上下
        if (currentGroupIndex >= transform.childCount - 1)
        {
            isTop = true;
        }
        if (currentGroupIndex <= 0)
        {
            isBottom = true;
        }
        //左右
        if (currentStageIndex >= 2)
        {
            isRight = true;
        }
        if (currentStageIndex <= 0)
        {
            isLeft = true;
        }
    }
}
