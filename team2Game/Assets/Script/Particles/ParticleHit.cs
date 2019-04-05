using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHit : MonoBehaviour
{
    [SerializeField, Header("Playerへのダメージ量")]
    int damage = 10;

    private void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.tag=="Player")
        {
            PlayerManager.PlayerDamage(damage);
        }
    }
}
