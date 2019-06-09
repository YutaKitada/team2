using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    
    [SerializeField]
    private float coefficient;   // 空気抵抗係数
    [SerializeField]
    private Vector3 windVel;     //風速

    private float currentTime = 0;
    private float maxTime = 5;

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player" )
        {
            // 相対速度計算
            var relativeVelocity = windVel - col.GetComponent<Rigidbody>().velocity;

            // 空気抵抗を与える
            col.GetComponent<Rigidbody>().AddForce(coefficient * relativeVelocity);
        }
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >=maxTime)
        {
            Destroy(gameObject);
        }
    }
}
