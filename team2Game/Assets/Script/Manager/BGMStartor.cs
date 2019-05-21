using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMStartor : MonoBehaviour
{
    [SerializeField]
    private int bgmNumber = 0;
    [SerializeField]
    private float bgmVolume = 1;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.PlayBGM(bgmNumber, bgmVolume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
