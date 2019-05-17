using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タウラスにダメージが通る弱点
/// </summary>
public class WeakPoint : MonoBehaviour
{
    Taurus parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.GetComponentInParent<Taurus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //弱点に星が当たったら、ボスにダメージ
        if (other.gameObject.tag.Contains("Star"))
        {
            if (!parent.isHit) return;

            parent.Damage(2);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Contains("Star"))
        {
            parent.isHit = true;
        }
    }
}
