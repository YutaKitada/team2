using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ImageChangeSelects : MonoBehaviour
{
    [SerializeField, Header("読み込むシーン名")]
    private string loadSceneName = "Stage0";            //初期値(変更可)

    //　非同期動作で使用するAsyncOperation
    private AsyncOperation async;
    ////　シーンロード中に表示するUI画面
    [SerializeField]
    private int bgmNumber = 0;

    //Active active;

    // Start is called before the first frame update
    void Start()
    {
        //同じGameObjectについている「Button」スクリプトを取得

    }
    private void Update()
    {
        //ボタンがクリックされたときに「シーンを読み込む機能」が呼ばれるように登録
        if (Input.GetButtonDown("AButton") || Input.GetKeyDown(KeyCode.Z))
        {
            //アクティブクラスで登録したもののアクティブをFalseに変える
            SoundManager.PlaySE(bgmNumber);
            loadSceneName = ImageSelect.currentStageName;
            NextScene();
        }
    }

    public void NextScene()
    {
        //　ロード画面UIをアクティブにする
        //loadUI.SetActive(true);
        //　コルーチンを開始
        StartCoroutine("LoadData");
    }

    /// <summary>
    /// シーンの読み込み中
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadData()
    {
        // シーンの読み込みをする
        async = SceneManager.LoadSceneAsync(loadSceneName);
        Time.timeScale = 1f;
        //　読み込みが終わるまで進捗状況をスライダーの値に反映させる
        while (!async.isDone)
        {
            var progressVal = Mathf.Clamp01(async.progress / 0.9f);
            //slider.value = progressVal;
            yield return null;
        }
    }
}
