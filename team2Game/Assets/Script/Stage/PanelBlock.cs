using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBlock : MonoBehaviour
{
    [SerializeField]
    private GameObject block;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            block.layer = 11;
            if(Input.GetAxisRaw("Vertical") <= -0.7f)
            {
                block.layer = 12;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            block.layer = 12;
        }
    }
}
