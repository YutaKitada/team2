using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
    [SerializeField]
    private Sprite[] comboNumberSprites;
    [SerializeField]
    private Image numberPosition_onePlace;
    [SerializeField]
    private Image numberPosition_tenPlace;
    [SerializeField]
    private Image comboText;

    [SerializeField]
    private float comboTime = 3;
    public static float comboTimer;

    // Start is called before the first frame update
    void Start()
    {
        numberPosition_onePlace.enabled = false;
        numberPosition_tenPlace.enabled = false;
        comboText.enabled = false;

        comboTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.isCombo)
        {
            numberPosition_onePlace.enabled = true;
            if(GameManager.combo >= 10)
            {
                numberPosition_tenPlace.enabled = true;
            }
            comboText.enabled = true;

            numberPosition_onePlace.sprite = comboNumberSprites[GameManager.combo % 10];
            if (numberPosition_tenPlace.enabled)
            {
                numberPosition_tenPlace.sprite = comboNumberSprites[(GameManager.combo - GameManager.combo % 10)/10];
            }

            if (!PlayerManager.isWishMode)
            {
                comboTimer += Time.deltaTime;
            }
            
            transform.localScale = new Vector3(1, 1, 1) / (comboTime / (comboTime - comboTimer));

            if(comboTimer >= comboTime)
            {
                UIManager.isCombo = false;
            }
        }
        else
        {
            numberPosition_onePlace.enabled = false;
            numberPosition_tenPlace.enabled = false;
            comboText.enabled = false;
            comboTimer = 0;
            GameManager.combo = 0;
        }
    }
}
