using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バウンドさせるクラス（モチーフ：魚座）
/// </summary>
public class Bound : MonoBehaviour
{
    Rigidbody rigid;

    [SerializeField]
    float boundPower = 5;
    public float BoundPower
    {
        get { return boundPower; }
        set { boundPower = value; }
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag.Contains("Stage"))
        {
            rigid.AddForce(Vector3.up * BoundPower, ForceMode.Impulse);
        }
    }
}
