using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //弱点に星が当たったら、ボスを無敵状態に移行
        if (other.gameObject.tag.Contains("Star"))
        {
            if (!parent.isHit) return;

            parent.Damage();
            parent.Stop();
            parent.isHit = false;
        }
    }
}
