using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMStarter : MonoBehaviour
{
    [SerializeField]
    private int bgmNumber = 0;
    [SerializeField,Range(0,1)]
    private float bgmVolume = 1;

    private bool start;

    // Start is called before the first frame update
    void Start()
    {
        start = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!start)
        {
            start = true;
            SoundManager.PlayBGM(bgmNumber, bgmVolume);
        }
    }
}
