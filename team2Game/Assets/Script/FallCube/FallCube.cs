using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallCube : MonoBehaviour
{
    public float FallTime;//落ちるまでの時間
    private bool FallFlag;//フラグ

    // Start is called before the first frame update
    void Start()
    {
        FallTime = 0;
        FallFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (FallFlag)
        {
            FallTime += Time.deltaTime;
        }

        if (FallTime > 3)//Playerが触れて３びょうが経過したら落ちる
        {
            transform.position += new Vector3(0, -9.8f, 0) * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)//あたったとき
    {
        if (collision.transform.tag == "Player")
        {
            FallFlag = true;
        }
    }
}
