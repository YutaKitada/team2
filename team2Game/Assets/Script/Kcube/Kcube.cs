using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kcube : MonoBehaviour
{

    private float destroytime = 0;
    private float burningtime = 0;
    private bool burningFlag = false;
    public Material BurningMaterial;
    private MeshRenderer render;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<MeshRenderer>();
        destroytime = 0;
        burningtime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (burningFlag == true)
        {
            render.material = BurningMaterial;

            destroytime += Time.deltaTime;
            if (destroytime > 5)
            {
                Destroy(gameObject);
            }
        }
        else destroytime = 0;
    }

    private void OnCollisionEnter(Collision collision)//あたったとき
    {
        if (collision.transform.tag == "Star")
        {
            burningFlag = true;
        }
    }

    private void OnCollisionStay(Collision collision)//あたっているとき
    {
        if (collision.transform.tag == "Kcube")
        {
            if (collision.gameObject.GetComponent<Kcube>().burningFlag)
            {
                burningtime += Time.deltaTime;
                if (burningtime > 1.5f)
                {
                    burningFlag = true;
                }
            }
        }
    }
}
