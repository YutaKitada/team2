using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossClear : MonoBehaviour
{
    GameObject bossEnemy;
    float elapsedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        bossEnemy = GameObject.FindGameObjectWithTag("BossEnemy");
    }

    // Update is called once per frame
    void Update()
    {
        if (bossEnemy != null) return;

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1)
        {
            SceneManager.LoadScene("GameClear");
        }
    }
}
