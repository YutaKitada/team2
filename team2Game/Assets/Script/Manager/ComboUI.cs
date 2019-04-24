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
    private Image numberPosition_hundredPlace;
    [SerializeField]
    private Image numberPosition_thousandPlace;
    [SerializeField]
    private Image numberPosition_tenthousandPlace;
    [SerializeField]
    private Image comboText;

    [SerializeField]
    private float comboTime = 3;
    public static float comboTimer;

    [SerializeField]
    private bool clearScene = false;

    public static bool comboTimerStop;

    // Start is called before the first frame update
    void Start()
    {
        numberPosition_onePlace.enabled = false;
        numberPosition_tenPlace.enabled = false;
        numberPosition_hundredPlace.enabled = false;
        numberPosition_thousandPlace.enabled = false;
        numberPosition_tenthousandPlace.enabled = false;
        comboText.enabled = false;

        comboTimer = 0;

        comboTimerStop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!clearScene)
        {
            if (UIManager.isCombo)
            {
                if (GameManager.combo > 99999)
                {
                    GameManager.combo = 99999;
                }

                numberPosition_onePlace.enabled = true;
                if (GameManager.combo >= 10)
                {
                    numberPosition_tenPlace.enabled = true;
                }
                if (GameManager.combo >= 100)
                {
                    numberPosition_hundredPlace.enabled = true;
                }
                if (GameManager.combo >= 1000)
                {
                    numberPosition_thousandPlace.enabled = true;
                }
                if (GameManager.combo >= 10000)
                {
                    numberPosition_tenthousandPlace.enabled = true;
                }
                comboText.enabled = true;

                numberPosition_onePlace.sprite = comboNumberSprites[GameManager.combo % 10];
                if (numberPosition_tenPlace.enabled)
                {
                    numberPosition_tenPlace.sprite = comboNumberSprites[(GameManager.combo / 10) % 10];
                }
                if (numberPosition_hundredPlace.enabled)
                {
                    numberPosition_hundredPlace.sprite = comboNumberSprites[(GameManager.combo / 100) % 10];
                }
                if (numberPosition_thousandPlace.enabled)
                {
                    numberPosition_thousandPlace.sprite = comboNumberSprites[(GameManager.combo / 1000) % 10];
                }
                if (numberPosition_tenthousandPlace.enabled)
                {
                    numberPosition_tenthousandPlace.sprite = comboNumberSprites[( GameManager.combo / 10000) % 10];
                }

                if (!PlayerManager.isWishMode && !comboTimerStop)
                {
                    comboTimer += Time.deltaTime;
                }

                transform.localScale = new Vector3(1, 1, 1) / (comboTime / (comboTime - comboTimer));

                if (comboTimer >= comboTime)
                {
                    UIManager.isCombo = false;
                }
            }
            else
            {
                numberPosition_onePlace.enabled = false;
                numberPosition_tenPlace.enabled = false;
                numberPosition_hundredPlace.enabled = false;
                numberPosition_thousandPlace.enabled = false;
                numberPosition_tenthousandPlace.enabled = false;
                comboText.enabled = false;
                comboTimer = 0;
                GameManager.combo = 0;
            }
        }
        else
        {
            numberPosition_onePlace.enabled = true;
            if (GameManager.maxCombo >= 10)
            {
                numberPosition_tenPlace.enabled = true;
            }
            if (GameManager.maxCombo >= 100)
            {
                numberPosition_hundredPlace.enabled = true;
            }
            if (GameManager.maxCombo >= 1000)
            {
                numberPosition_thousandPlace.enabled = true;
            }
            if (GameManager.maxCombo >= 10000)
            {
                numberPosition_tenthousandPlace.enabled = true;
            }
            comboText.enabled = true;

            numberPosition_onePlace.sprite = comboNumberSprites[GameManager.maxCombo % 10];
            if (numberPosition_tenPlace.enabled)
            {
                numberPosition_tenPlace.sprite = comboNumberSprites[(GameManager.maxCombo / 10) % 10];
            }
            if (numberPosition_hundredPlace.enabled)
            {
                numberPosition_hundredPlace.sprite = comboNumberSprites[(GameManager.maxCombo / 100) % 10];
            }
            if (numberPosition_thousandPlace.enabled)
            {
                numberPosition_thousandPlace.sprite = comboNumberSprites[(GameManager.maxCombo / 1000) % 10];
            }
            if (numberPosition_tenthousandPlace.enabled)
            {
                numberPosition_tenthousandPlace.sprite = comboNumberSprites[(GameManager.maxCombo / 10000) % 10];
            }
        }
    }
}
