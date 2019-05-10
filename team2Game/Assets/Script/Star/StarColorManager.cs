using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarColorManager : MonoBehaviour
{
    [SerializeField]
    private List<Material> materialList;                                //色用マテリアルList

    public enum StarCurrent {LOW,MEDIUMLOW,MEDIUM,MEDIUMHIGH,HIGH};     //星の状態
    [HideInInspector]
    public StarCurrent starCurrent;                                     //星の状態取得用

    private MeshRenderer render;                                        //星のMeshRendere、色の変化用

    private Color materialColor;                                        //星の色

    [SerializeField]
    private GameObject starLight;

    // Start is called before the first frame update
    void Start()
    {
        starCurrent = StarCurrent.HIGH;             //最初は高温

        render = GetComponent<MeshRenderer>();      //MeshRenderer取得

        materialColor = materialList[4].color;      //高温の状態に色を変更

        starLight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //温度によって状態を変える
        if(UIManager.hpGageFillAmount < 20)
        {
            starCurrent = StarCurrent.LOW;
        }
        else if (UIManager.hpGageFillAmount < 40)
        {
            starCurrent = StarCurrent.MEDIUMLOW;
        }
        else if(UIManager.hpGageFillAmount < 60)
        {
            starCurrent = StarCurrent.MEDIUM;
        }
        else if(UIManager.hpGageFillAmount < 80)
        {
            starCurrent = StarCurrent.MEDIUMHIGH;
        }
        else
        {
            starCurrent = StarCurrent.HIGH;
        }


        //色の状態に応じて色の変化を開始
        switch (starCurrent)
        {
            case StarCurrent.HIGH:
                //render.material = materialList[4];
                materialColor = Color.Lerp(materialList[4].color,materialList[3].color,(100- UIManager.hpGageFillAmount)/20);
                render.material.color = materialColor;
                break;
            case StarCurrent.MEDIUMHIGH:
                //render.material = materialList[3];
                materialColor = Color.Lerp(materialList[3].color, materialList[2].color, (80 - UIManager.hpGageFillAmount) / 20);
                render.material.color = materialColor;
                break;
            case StarCurrent.MEDIUM:
                //render.material = materialList[2];
                materialColor = Color.Lerp(materialList[2].color, materialList[1].color, (60 - UIManager.hpGageFillAmount) / 20);
                render.material.color = materialColor;
                break;
            case StarCurrent.MEDIUMLOW:
                //render.material = materialList[1];
                materialColor = Color.Lerp(materialList[1].color, materialList[0].color, (40 - UIManager.hpGageFillAmount) / 20);
                render.material.color = materialColor;
                break;
            case StarCurrent.LOW:
                //render.material = materialList[0];
                materialColor = Color.Lerp(materialList[0].color, materialList[5].color, (20 - UIManager.hpGageFillAmount) / 20);
                render.material.color = materialColor;
                break;
        }

        if (!PlayerManager.isWishMode)
        {
            starLight.SetActive(false);
        }
        else
        {
            starLight.SetActive(true);
        }
    }
}
