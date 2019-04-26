using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        //他のGameObjectにアタッチされているか調べる
        if (this != Instance)
        {
            Destroy(this);

            //SceneをまたいでこのGameObjectを有効にしたい場合
            //DontDestroyOnLoad(gameObject);
        }
    }
}