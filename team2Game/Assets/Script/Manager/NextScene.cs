using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [SerializeField]
    private string buttonName;          //押すボタンの名前
    [SerializeField]
    private string nextSceneName;       //次のシーンの名前

    private bool titleScene;
    

    // Start is called before the first frame update
    void Start()
    {
        titleScene = false;
    }

    // Update is called once per frame
    void Update()
    {
        //エスケープキーが押されたらゲーム終了
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //ボタンを押したら次のシーンへ
        if (Input.GetButtonDown(buttonName) && !titleScene)
        {
            if(SceneManager.GetActiveScene().name == "Title")
            {
                titleScene = true;
                SoundManager.PlayVOICE(Random.Range(0, 3));
            }
            else
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }

        if (titleScene)
        {
            if (!SoundManager.CheckPlayVOICE())
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
