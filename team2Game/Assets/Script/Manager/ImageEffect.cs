using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageEffect : MonoBehaviour
{

    enum ImageEffectState { SPRITERENDERE, IMAGE, NULL }
    private ImageEffectState imageState;

    private Image image;
    private SpriteRenderer render;

    //点滅用
    [SerializeField]
    private float flashingTime = 5;
    private float flashingTimer;
    private float alphaChangeTimer;
    [SerializeField]
    private bool flashingFlag;

    //半透明用
    [SerializeField]
    private float invisibleTime = 5;
    private float invisibleTimer;
    [SerializeField]
    private bool invisibleFlag;

    //回転用
    private float rotationAmount;
    [SerializeField]
    private float rotationSpeed = 5;
    [SerializeField]
    private bool rotationFlag;

    //サイズ変換用
    private float scaleChangeTimer;
    [SerializeField]
    private bool scaleChangeFlag;
    private bool big;

    // Use this for initialization
    void Start()
    {
        if (GetComponent<Image>() != null)
        {
            image = GetComponent<Image>();
            imageState = ImageEffectState.IMAGE;
        }
        else if (GetComponent<SpriteRenderer>() != null)
        {
            render = GetComponent<SpriteRenderer>();
            imageState = ImageEffectState.SPRITERENDERE;
        }
        else
        {
            Debug.Log("コンポーネントが見つかりません。\nこのスクリプトはImageもしくはSpriteRendererを持つオブジェクトに使用してください。");
            imageState = ImageEffectState.NULL;
        }

        //flashingFlag = false;
        flashingTimer = 0;
        alphaChangeTimer = 0;

        //invisibleFlag = false;
        invisibleTimer = 0;

        //rotationFlag = false;
        rotationAmount = 0;

        big = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (imageState != ImageEffectState.NULL)
        {
            Flashing();
            Invisible();
            Rotation();
            ScaleChange();
        }
    }

    //点滅メソッド
    private void Flashing()
    {
        if (flashingFlag)
        {
            alphaChangeTimer += Time.deltaTime;
            //flashingTimer += Time.deltaTime;
            if(flashingTimer < flashingTime)
            {
                if (alphaChangeTimer >= 0.75f)
                {
                    switch (imageState)
                    {
                        case ImageEffectState.IMAGE:
                            if (invisibleFlag)
                            {
                                if (image.color.a == 0)
                                {
                                    image.color = new Color(1, 1, 1, 0.5f);
                                }
                                else if (image.color.a == 0.5f)
                                {
                                    image.color = new Color(1, 1, 1, 0);
                                }
                            }
                            else if (!invisibleFlag)
                            {
                                if (image.color.a == 0)
                                {
                                    image.color = new Color(1, 1, 1, 1);
                                }
                                else if (image.color.a == 1)
                                {
                                    image.color = new Color(1, 1, 1, 0);
                                }
                            }
                            break;
                        case ImageEffectState.SPRITERENDERE:
                            if (invisibleFlag)
                            {
                                if (render.color.a == 0)
                                {
                                    render.color = new Color(1, 1, 1, 0.5f);
                                }
                                else if (render.color.a == 0.5f)
                                {
                                    render.color = new Color(1, 1, 1, 0);
                                }
                            }
                            else if (!invisibleFlag)
                            {
                                if (render.color.a == 0)
                                {
                                    render.color = new Color(1, 1, 1, 1);
                                }
                                else if (render.color.a == 1)
                                {
                                    render.color = new Color(1, 1, 1, 0);
                                }
                            }
                            break;
                    }
                    alphaChangeTimer = 0;
                }
            }
            else if(flashingTimer >= flashingTime)
            {
                flashingTimer = 0;
                alphaChangeTimer = 0;
                flashingFlag = false;
            }
            
        }
        else if (!flashingFlag)
        {
            switch (imageState)
            {
                case ImageEffectState.IMAGE:
                    if (image.color.a == 0)
                    {
                        image.color = new Color(1, 1, 1, 1);
                    }
                    break;
                case ImageEffectState.SPRITERENDERE:
                    if (render.color.a == 0)
                    {
                        render.color = new Color(1, 1, 1, 1);
                    }
                    break;
            }
        }
    }

    //半透明メソッド
    private void Invisible()
    {
        if (invisibleFlag)
        {
            invisibleTimer += Time.deltaTime;
            if(invisibleTimer < invisibleTime)
            {
                switch (imageState)
                {
                    case ImageEffectState.IMAGE:
                        if (image.color.a == 1)
                        {
                            image.color = new Color(1, 1, 1, 0.5f);
                        }
                        break;
                    case ImageEffectState.SPRITERENDERE:
                        if (render.color.a == 1)
                        {
                            render.color = new Color(1, 1, 1, 0.5f);
                        }
                        break;
                }
            }
            else if(invisibleTimer >= invisibleTime)
            {
                invisibleFlag = false;
                invisibleTimer = 0;
            }
        }
        else if (!invisibleFlag)
        {
            switch (imageState)
            {
                case ImageEffectState.IMAGE:
                    if (image.color.a == 0.5f)
                    {
                        image.color = new Color(1, 1, 1, 1);
                    }
                    break;
                case ImageEffectState.SPRITERENDERE:
                    if (render.color.a == 0.5f)
                    {
                        render.color = new Color(1, 1, 1, 1);
                    }
                    break;
            }
        }
    }

    //回転メソッド
    private void Rotation()
    {
        if (rotationFlag)
        {
            if (rotationAmount < 360)
            {
                rotationAmount += rotationSpeed;
                transform.Rotate(new Vector3(0, rotationSpeed, 0));
            }
            else if (rotationAmount >= 360)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
                rotationAmount = 0;
                rotationFlag = false;
            }
        }
        else if (!rotationFlag)
        {
            rotationAmount = 0;
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    private void ScaleChange()
    {
        if (scaleChangeFlag)
        {
            scaleChangeTimer += Time.deltaTime;

            Vector3 max = new Vector3(1, 1, 1);
            Vector3 min = new Vector3(0.3f, 0.3f, 0.3f);

            if (scaleChangeTimer < 1)
            {
                if (!big)
                {
                    transform.localScale = Vector3.Lerp(min, max, scaleChangeTimer);
                }
                else if (big)
                {
                    transform.localScale = Vector3.Lerp(max, min, scaleChangeTimer);
                }
            }
            else
            {
                scaleChangeTimer = 0;
                big = !big;
            }
        }
        
    }
}
