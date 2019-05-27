using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField]
    private Vector3 vector3;
    private void OnTriggerStay(Collider col)
    {
       
        if (col.transform.tag == "Player")
        {
            col.transform.GetComponent<PlayerController>().Damage(vector3);
        }
    }
}
