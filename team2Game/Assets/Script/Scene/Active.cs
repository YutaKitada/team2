using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject[] activeObjects;
    //配列内のオブジェクトを全てのアクティブを変更する
    public void ActiveObject(bool switchActive)
    {
        for (int i = 0; i < activeObjects.Length; i++)
        {
            activeObjects[i].SetActive(switchActive);
        }
    }

}
