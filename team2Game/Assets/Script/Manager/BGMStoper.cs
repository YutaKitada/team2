using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMStoper : MonoBehaviour
{
    [SerializeField]
    private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        if (stop)
        {
            SoundManager.StopBGM();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
