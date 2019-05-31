using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColorManager : MonoBehaviour
{
    private Color firstLightColor;      //初期のライトの色
    [SerializeField]
    private Color wishLightColor;       //願い事中のライトの色

    private Light myLight;                //自身が持つライトのコンポーネント

    // Start is called before the first frame update
    void Start()
    {
        myLight = GetComponent<Light>();
        firstLightColor = myLight.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.isWishMode)
        {
            myLight.color = firstLightColor;
        }
        else
        {
            myLight.color = wishLightColor;
        }

        if (WishManager.isMeteorShower || WishManager.wishProductionFlag|| GameManager.isOver)
        {
            myLight.color = wishLightColor;
        }
    }
}
