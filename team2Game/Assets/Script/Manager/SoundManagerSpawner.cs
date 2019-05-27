using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject soundManager;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("SoundManager2(Clone)") == null)
        {
            Instantiate(soundManager);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
