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
        if (!timerStop)
        {
            timer += Time.deltaTime;
        }
        else if (timerStop)
        {
            //image.color = new Color(1, 1, 1, 1);
        }

        if(timer >= time)
        {
            //image.color -= new Color(0, 0, 0, Time.deltaTime / 2);
            SoundManager.FadeOut();
            //if (image.color.a < 0)
            //{
                SceneManager.LoadScene(backSceneName);
            //}
        }

        if (Input.GetButtonDown("AButton"))
        {
            timerStop = true;
        }
    }
}
