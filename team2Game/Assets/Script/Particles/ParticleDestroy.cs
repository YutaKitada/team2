using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パーティクル再生終了後の削除クラス
/// </summary>
public class ParticleDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        Destroy(gameObject, particle.main.duration);
    }
}
