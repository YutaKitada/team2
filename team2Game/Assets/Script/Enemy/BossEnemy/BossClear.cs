using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossClear : MonoBehaviour
{
    GameObject[] bossEnemy;
    public static int bossEnemyCount;
    float elapsedTime = 0;

    [SerializeField]
    string sceneName = "GameClear";

    // Start is called before the first frame update
    void Start()
    {
        bossEnemy = GameObject.FindGameObjectsWithTag("BossEnemy");
        bossEnemyCount = bossEnemy.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (bossEnemyCount > 0) return;
        

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public static void SubCount(int count)
    {
        bossEnemyCount -= count;
    }
}
