using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private GameObject player;//Player用のゲームオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");//Playerを見つける
    }

    // Update is called once per frame
    void Update()
    {
        //-Physics.gravity.y
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            //other.transform.GetComponent<Rigidbody>().velocity += new Vector3(0, -Physics.gravity.y*Time.deltaTime);
            other.transform.GetComponent<Rigidbody>().useGravity = false;
            //Physics.gravity = Vector3.up;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
