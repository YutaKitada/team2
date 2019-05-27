using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteImage : MonoBehaviour
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
    private bool isLeft;
    private bool isRight;

    private bool upCount;
    private bool downCount;
    


    void Start()
    {
        //stageList[0][0]が最初に選んでいるものとする
        //currentGroupIndex = storageGroup;
        //currentStageIndex = storageStage;
        currentGroupIndex = 0;
        currentStageIndex = 0;

        StageListInit();
        upCount = false;
        downCount = false;
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
        TestFunc3();
    }

    //選択の横移動
    private void MoveHorizon()
    {
        if (LeftStick.Instance.IsRightDown() && !isRight)
        {
            isLeft = false;
            //選択しているStageを変更する
            currentGroupIndex++;
        }
        if (LeftStick.Instance.IsLeftDown() && !isLeft)
        {
            isRight = false;
            //選択しているStageを変更する
            currentGroupIndex--;
        }
    }

    //選択の縦移動
    private void MoveVertical()
    {
        if (upCount)
        {
           
            //選択しているGroupを変更する
            currentStageIndex++;
            if (currentStageIndex >= stageList[0].Count)
            {
                currentStageIndex = 0;
            }
            upCount = false;
        }
        //下を押されてかつ要素数が0でないとき
        if (downCount)
        {
            if (currentStageIndex >= 1)
            {
                //選択しているGroupを変更する
                currentStageIndex--;
                downCount = false;
                return;
            }
            if (currentStageIndex == 0)
            {
                currentStageIndex = stageList[0].Count - 1;
                downCount = false;
                return;
            }
            
        }
    }

    //全てのステージのスケールを0にする
    private void TestFunc1()
    {
        foreach (var group in stageList)
        {
            foreach (var stage in group)
            {
                Image image = stage.GetComponent<Image>();
                image.transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }
    //選択されている要素とその前後2つ以外を非表示にする
    private void TestFunc3()
    {
        //選択番号が0の時 下ならべ
        if(currentStageIndex <=0)
        {
            UpdateImageSelects(8, 7);
        }
        else if(currentStageIndex ==1)
        {
            UpdateImageSelects(currentStageIndex - 1, 8);
        }
        else
        {
            UpdateImageSelects(currentStageIndex - 1, currentStageIndex - 2);
        }
        //選択番号が8の時　上ならべ
        if (currentStageIndex >= 8)
        {
            UpdateImageSelects(0, 1);
        }
        else if (currentStageIndex == 7)
        {
            UpdateImageSelects(currentStageIndex + 1, 0);
        }
        else
        {
            UpdateImageSelects(currentStageIndex + 1, currentStageIndex + 2);
        }
    }


    private void UpdateImageSelects( int index1, int index2)
    {
        Image image = stageList[currentGroupIndex][index1].GetComponent<Image>();
        image.transform.localScale = new Vector3(1, 1, 0);

        Image image2 = stageList[currentGroupIndex][index2].GetComponent<Image>();
        image2.transform.localScale = new Vector3(1, 1, 0);
    }

    //現在選択中のステージの色を変える
    private void TestFunc2()
    {
        Image image = stageList[currentGroupIndex][currentStageIndex].GetComponent<Image>();
        image.transform.SetAsLastSibling();
        currentStageName = image.transform.GetChild(0).gameObject.name;

    }

    //選択範囲を決める
    private void Clamp()
    {
        //左右
        if (currentGroupIndex >= stageList.Count-1)
        {
            isRight = true;
        }
        if (currentGroupIndex <= 0)
        {
            isLeft = true;
        }
    }

    //外から判断する
    public bool UpCount(bool isUp)
    {
        return upCount = isUp;
    }
    public bool DownCount(bool isDown)
    {
        return downCount = isDown;
    }

    //現在の選択番号
    public int CurrentNum()
    {
        return currentStageIndex+1;
    }
}
