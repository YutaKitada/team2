using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHit : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.tag=="Player")
        {
            PlayerManager.PlayerDamage(5);
            Debug.Log("HitPlayer");
        }
    }
}
