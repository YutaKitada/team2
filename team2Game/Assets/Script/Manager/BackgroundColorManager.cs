using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundColorManager : MonoBehaviour
{
    private Color firstColor;      //初期のライトの色
    [SerializeField]
    private Color wishColor;       //願い事中のライトの色

    private Image myImage;                //自身が持つライトのコンポーネント

    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<Image>();
        firstColor = myImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.isWishMode)
        {
            myImage.color = firstColor;
        }
        else
        {
            myImage.color = wishColor;
        }

        if (WishManager.isMeteorShower || WishManager.wishProductionFlag || GameManager.isOver)
        {
            myImage.color = wishColor;
        }
    }
}
