using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SelectButton : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons;

    public static int currentButton = -1;    //最後に押されたボタン

    // Start is called before the first frame update
    void Start()
    {
        //ゲーム開始時は0番目のボタンが初期選択状態
        //currentButtonの値で変化
        if (currentButton == -1)
        {
            buttons[0].Select();
        }
        else
        {
            buttons[currentButton].Select();
        }
    }
    //インスペクターで変更可(ボタンの配列順で番号を指定)
    public void OnClick(int num)
    {
        currentButton = num;
    }
    
}
