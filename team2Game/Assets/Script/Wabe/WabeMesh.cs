using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WabeMesh : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            
            transform.parent.GetComponent<MyMesh>().isPlayer = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            transform.parent.GetComponent<MyMesh>().isPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            transform.parent.GetComponent<MyMesh>().isPlayer = false;
            transform.parent.GetComponent<MyMesh>().WabeMotionFlag = true;
        }
    }
}
