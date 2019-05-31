using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossClear : MonoBehaviour
{
    GameObject[] bossEnemy;
    public static int bossEnemyCount;
    float elapsedTime = 0;

    [SerializeField]
    string sceneName = "GameClear";

    [SerializeField]
    GameObject panel;
    Image image;

    float red, blue, green, alpha;
    float speed = 0.01f;

    bool isCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        bossEnemy = GameObject.FindGameObjectsWithTag("BossEnemy");
        bossEnemyCount = bossEnemy.Length;

        image = panel.GetComponent<Image>();
        red = image.color.r;
        blue = image.color.b;
        green = image.color.g;
        alpha = image.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if (bossEnemyCount > 0) return;

        SceneChange();
        DenebSceneChange();
    }

    void SceneChange()
    {
        if (SceneManager.GetActiveScene().name == "stage3-3") return;

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    void DenebSceneChange()
    {
        if (SceneManager.GetActiveScene().name != "stage3-3") return;

        FadeOut();
        if (isCompleted) SceneManager.LoadScene(sceneName);
    }

    public static void SubCount(int count)
    {
        bossEnemyCount -= count;
    }

    void FadeOut()
    {
        if (alpha >= 1)
        {
            isCompleted = true;
            return;
        }

        image.color = new Color(red, green, blue, alpha);
        alpha += speed;
    }
}
