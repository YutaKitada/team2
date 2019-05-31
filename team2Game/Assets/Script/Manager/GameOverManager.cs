using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverImage;
    [SerializeField]
    private GameObject selectObject;
    [SerializeField]
    private GameObject ui;

    private float timer;
    private Vector3 firstPosition;

    // Start is called before the first frame update
    void Start()
    {
        gameOverImage.transform.localPosition = new Vector3(gameOverImage.transform.localPosition.x, 360);
        
        firstPosition = gameOverImage.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isOver)
        {
            if (ui.activeSelf)
            {
                ui.SetActive(false);
                
            }
            timer += Time.deltaTime;

            if(timer <= 3)
            {
                gameOverImage.transform.localPosition = Vector3.Lerp(firstPosition, new Vector3(gameOverImage.transform.localPosition.x,60), timer/3);
            }
            else
            {
                if (!selectObject.activeSelf)
                {
                    selectObject.SetActive(true);
                }
                
            }
        }
        else
        {
            selectObject.SetActive(false);
        }
    }
}
