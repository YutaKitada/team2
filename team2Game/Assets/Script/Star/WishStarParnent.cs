using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishStarParnent : MonoBehaviour
{
    private Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        rigid.velocity = new Vector3(10, -10);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -30)
        {
            Destroy(gameObject);
        }
    }
}
