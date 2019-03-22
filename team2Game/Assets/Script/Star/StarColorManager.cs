using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarColorManager : MonoBehaviour
{
    [SerializeField]
    private List<Material> materialList;

    public enum StarCurrent {LOW,MEDIUMLOW,MEDIUM,MEDIUMHIGH,HIGH};
    [HideInInspector]
    public StarCurrent starCurrent;

    private MeshRenderer render;

    private Color materialColor;

    // Start is called before the first frame update
    void Start()
    {
        starCurrent = StarCurrent.HIGH;

        render = GetComponent<MeshRenderer>();

        materialColor = materialList[4].color;
    }

    // Update is called once per frame
    void Update()
    {
        if(UIManager.gageFillAmount < 20)
        {
            starCurrent = StarCurrent.LOW;
        }
        else if (UIManager.gageFillAmount < 40)
        {
            starCurrent = StarCurrent.MEDIUMLOW;
        }
        else if(UIManager.gageFillAmount < 60)
        {
            starCurrent = StarCurrent.MEDIUM;
        }
        else if(UIManager.gageFillAmount < 80)
        {
            starCurrent = StarCurrent.MEDIUMHIGH;
        }
        else
        {
            starCurrent = StarCurrent.HIGH;
        }


        switch (starCurrent)
        {
            case StarCurrent.HIGH:
                //render.material = materialList[4];
                materialColor = Color.Lerp(materialList[4].color,materialList[3].color,(100- UIManager.gageFillAmount)/20);
                render.material.color = materialColor;
                break;
            case StarCurrent.MEDIUMHIGH:
                //render.material = materialList[3];
                materialColor = Color.Lerp(materialList[3].color, materialList[2].color, (80 - UIManager.gageFillAmount) / 20);
                render.material.color = materialColor;
                break;
            case StarCurrent.MEDIUM:
                //render.material = materialList[2];
                materialColor = Color.Lerp(materialList[2].color, materialList[1].color, (60 - UIManager.gageFillAmount) / 20);
                render.material.color = materialColor;
                break;
            case StarCurrent.MEDIUMLOW:
                //render.material = materialList[1];
                materialColor = Color.Lerp(materialList[1].color, materialList[0].color, (40 - UIManager.gageFillAmount) / 20);
                render.material.color = materialColor;
                break;
            case StarCurrent.LOW:
                //render.material = materialList[0];
                materialColor = Color.Lerp(materialList[0].color, materialList[5].color, (20 - UIManager.gageFillAmount) / 20);
                render.material.color = materialColor;
                break;
        }
    }
}
