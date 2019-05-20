using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoManager : MonoBehaviour
{
    private VideoPlayer video;

    [SerializeField]
    private string nextSceneName;

    private bool skip;

    // Start is called before the first frame update
    void Start()
    {
        video = GetComponent<VideoPlayer>();
        skip = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("AButton"))
        {
            skip = true;
        }

        if (!video.isPlaying || skip)
        {
            video.targetCameraAlpha -= Time.deltaTime / 2;
            if(video.targetCameraAlpha < 0)
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
