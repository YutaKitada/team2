using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoMovieScene : MonoBehaviour
{
    [SerializeField]
    private float time = 15;
    private float timer;
    private bool timerStop;

    [SerializeField]
    private string backSceneName = "MovieScene";

    //[SerializeField]
    //private Image image;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        timerStop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("AButton"))
        {
            SceneManager.LoadScene(backSceneName);
        }
    }
}
