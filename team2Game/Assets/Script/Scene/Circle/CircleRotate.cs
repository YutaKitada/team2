using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotate : MonoBehaviour
{
    RouletteImage roulette;
    private bool isUp;
    private bool isDown;
    //移動中用
    private bool isMove;
    private float radius;


    // Start is called before the first frame update
    void Start()
    {
        roulette = GetComponent<RouletteImage>();
        isMove = false;
        isDown = false;
        isUp = false;
    }
    

    // Update is called once per frame
    void Update()
    {
        if(LeftStick.Instance.IsTop() && !isUp)
        {
            isMove = true;
            isDown = true;
            isUp = false;
            
        }
        if (LeftStick.Instance.IsBottom() && !isDown)
        {
            isMove = true;
            isUp = true;
            isDown = false;
        }
        Rote();
    }

    void Rote()
    {
        if (isDown)
        {
            radius+=4;　 transform.Rotate(new Vector3(-radius / 5.5f, 0, 0));
            //radius+=2;   transform.Rotate(new Vector3(-radius / 10.5f, 0, 0));
            //radius ++;   transform.Rotate(new Vector3(-radius / 20.5f, 0, 0));
            if (radius >=40)
            {
                isDown = false;
                isMove = false;
                radius = 0;
                roulette.UpCount(true);
            }
        }

        if (isUp)
        {
            radius += 4;   transform.Rotate(new Vector3(radius / 5.5f, 0, 0));
            //radius+=2;     transform.Rotate(new Vector3(radius / 10.5f, 0, 0));
            // radius ++;    transform.Rotate(new Vector3(radius / 20.5f, 0, 0));
            if (radius >= 40)
            {
                isUp = false;
                isMove = false;
                radius = 0;
                roulette.DownCount(true);
            }
        }
    }
}
