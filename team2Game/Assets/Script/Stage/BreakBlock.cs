using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour
{

    [SerializeField]
    private float breakDistance = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.tag == "Star")
        {
            if (!GameManager.star.GetComponent<StarMovement>().returnPlayer)
            {
                if (breakDistance != 0)
                {
                    float distace = Vector3.Distance(PlayerManager.throwPosition, transform.position);
                    if (distace >= breakDistance)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
