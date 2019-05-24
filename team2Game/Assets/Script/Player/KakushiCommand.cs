using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class KakushiCommand : MonoBehaviour
{
    int cmdSeq = 0;
    int[] keyCodes;
    int[] konamiCommand = new[] {
        (int)KeyCode.UpArrow,
        (int)KeyCode.UpArrow,
        (int)KeyCode.DownArrow,
        (int)KeyCode.DownArrow,
        (int)KeyCode.LeftArrow,
        (int)KeyCode.RightArrow,
        (int)KeyCode.LeftArrow,
        (int)KeyCode.RightArrow,
        (int)KeyCode.B,
        (int)KeyCode.A
    };
    int kcnt = 0;

    [SerializeField]
    private GameObject spaceCancer;

    private void Start()
    {
        keyCodes = (int[])Enum.GetValues(typeof(KeyCode));
    }

    void Update()
    {
        var len = keyCodes.Length;
        for (var i = 0; i < len; i++)
        {
            if (Input.GetKeyUp((KeyCode)keyCodes[i]))
            {
                if (konamiCommand[cmdSeq] == keyCodes[i])
                {
                    cmdSeq++;
                    if (cmdSeq == konamiCommand.Length)
                    {
                        kcnt++;
                        int spawn = 50;
                        while(spawn > 0)
                        {
                            Instantiate(spaceCancer, GameManager.player.transform.position + new Vector3(0, 30), Quaternion.identity);
                            spawn--;
                        }
                        cmdSeq = 0;
                    }
                }
                else
                {
                    cmdSeq = 0;
                }
            }
        }
    }
}