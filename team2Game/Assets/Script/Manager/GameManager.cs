using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int combo;
    public static GameObject player;
    public static GameObject star;

    public static bool isClear;
    public static bool isOver;

    // Start is called before the first frame update
    void Awake()
    {
        combo = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        star = GameObject.FindGameObjectWithTag("Star");
        isClear = false;
        isOver = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (!SoundManager.CheckBGM(1))
        {
            SoundManager.PlayBGM(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            UIManager.gageFillAmount = 100;
        }

        if (isOver)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
