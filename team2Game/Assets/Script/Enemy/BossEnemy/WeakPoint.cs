using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    BossEnemy1 parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.GetComponentInParent<BossEnemy1>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Star"))
        {
            if (!parent.isHit) return;

            parent.Damage();
            parent.Stop();
            parent.mode = Mode.INVINCIBLE;
            parent.isHit = false;
        }
    }
}
