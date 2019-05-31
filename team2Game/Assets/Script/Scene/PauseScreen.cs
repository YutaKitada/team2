using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseScreen : MonoBehaviour
{

    //子の動きを停止させる
    [SerializeField]
    private GameObject pauseUIPrefab;     //Pauseのプレハブを入れる
    private GameObject pauseUIInstance;　 //Pauseのプレハブ代入用

    [SerializeField]
    private GameObject selects;　　　　   //activeにするものbuttonなど

    public class RigidbodyVelocity
    {
        public Vector3 velocity;
        public Vector3 angularVeloccity;
        public RigidbodyVelocity(Rigidbody rigidbody)
        {
            velocity = rigidbody.velocity;
            angularVeloccity = rigidbody.angularVelocity;
        }
    }



    /// <summary>
    /// 現在Pause中か？
    /// </summary>
    //[SerializeField]
    public static bool pausing = false;

    private void Start()
    {
        pausing = false;
    }

    /// <summary>
    /// 無視するGameObject
    /// </summary>
    [SerializeField]
    private GameObject[] ignoreGameObjects;

    /// <summary>
    /// ポーズ状態が変更された瞬間を調べるため、前回のポーズ状況を記録しておく
    /// </summary>
    bool prevPausing;

    /// <summary>
    /// Rigidbodyのポーズ前の速度の配列
    /// </summary>
    RigidbodyVelocity[] rigidbodyVelocities;

    /// <summary>
    /// ポーズ中のRigidbodyの配列
    /// </summary>
    Rigidbody[] pausingRigidbodies;

    /// <summary>
    /// ポーズ中のMonoBehaviourの配列
    /// </summary>
    MonoBehaviour[] pausingMonoBehaviours;

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // ポーズ状態が変更されていたら、Pause/Resumeを呼び出す。

        if (Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            //子オブジェクトの動きを止める　もう一度押されたら再開する
            if (pausing == false)
            {
                pausing = true;
            }
            else
            {
                pausing = false;
            }


            if (prevPausing != pausing)
            {
                if (pausing) Pause();
                else Resume();
                prevPausing = pausing;
            }
            //Pauseの画面を出す、同時にボタンをActiveにするもう一度押されたらFalseにする
            if (pauseUIInstance == null)
            {
                pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
                selects.SetActive(true);
            }
            else
            {
                Destroy(pauseUIInstance);
                selects.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 中断
    /// </summary>
    void Pause()
    {
        // Rigidbodyの停止
        // 子要素から、スリープ中でなく、IgnoreGameObjectsに含まれていないRigidbodyを抽出
        Predicate<Rigidbody> rigidbodyPredicate =
            obj => !obj.IsSleeping() &&
                   Array.FindIndex(ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;
        pausingRigidbodies = Array.FindAll(transform.GetComponentsInChildren<Rigidbody>(), rigidbodyPredicate);
        rigidbodyVelocities = new RigidbodyVelocity[pausingRigidbodies.Length];
        for (int i = 0; i < pausingRigidbodies.Length; i++)
        {
            // 速度、角速度も保存しておく
            rigidbodyVelocities[i] = new RigidbodyVelocity(pausingRigidbodies[i]);
            pausingRigidbodies[i].Sleep();
        }

        // MonoBehaviourの停止
        // 子要素から、有効かつこのインスタンスでないもの、IgnoreGameObjectsに含まれていないMonoBehaviourを抽出
        Predicate<MonoBehaviour> monoBehaviourPredicate =
            obj => obj.enabled &&
                   obj != this &&
                   Array.FindIndex(ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;
        pausingMonoBehaviours = Array.FindAll(transform.GetComponentsInChildren<MonoBehaviour>(), monoBehaviourPredicate);
        foreach (var monoBehaviour in pausingMonoBehaviours)
        {
            monoBehaviour.enabled = false;
        }

    }

    /// <summary>
    /// 再開
    /// </summary>
    void Resume()
    {
        // Rigidbodyの再開
        for (int i = 0; i < pausingRigidbodies.Length; i++)
        {
            pausingRigidbodies[i].WakeUp();
            pausingRigidbodies[i].velocity = rigidbodyVelocities[i].velocity;
            pausingRigidbodies[i].angularVelocity = rigidbodyVelocities[i].angularVeloccity;
        }

        // MonoBehaviourの再開
        foreach (var monoBehaviour in pausingMonoBehaviours)
        {
            monoBehaviour.enabled = true;
        }
    }
}
