using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バウンドさせるクラス
/// </summary>
public class Bound : MonoBehaviour
{
    Rigidbody rigid;
    [SerializeField]
    float boundPower = 5;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.name.Contains("Floor"))
        {
            rigid.AddForce(Vector3.up * boundPower, ForceMode.Impulse);
        }
    }
}
