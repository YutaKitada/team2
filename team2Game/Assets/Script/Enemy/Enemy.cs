using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemyの大元
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    protected Rigidbody rigid;
    protected Quaternion rotation;

    [SerializeField, Header("移動量")]
    protected float power = 10f;
    protected float maxSpeed = 0f;//最大移動スピード(Startメソッドで決定)

    [SerializeField]
    protected Transform target;

    [SerializeField, Header("目的地（設定しない場合は0）"), Range(0, 5)]
    protected float destinationPosition;
    protected float startPosition;//初期位置

    protected Vector3 distance;//targetとの距離

    protected Vector3 forward = new Vector3(0, -90, 0);//正面

    [SerializeField]
    protected int hp = 1;

    //左右移動用のbool
    public bool Direction_Left
    {
        get;
        set;
    } = true;

    //上下移動用のbool
    public bool Direction_Up
    {
        get;
        set;
    } = true;

    /// <summary>
    /// 移動処理
    /// </summary>
    public virtual void Move()
    {

    }

    /// <summary>
    /// 移動方向の設定
    /// </summary>
    public virtual void Direction()
    {

    }

    /// <summary>
    /// 弾を撃つ処理
    /// </summary>
    public virtual void Shot()
    {

    }

    /// <summary>
    /// ダメージを受けたときの処理
    /// </summary>
    public virtual void Damage()
    {

    }

    /// <summary>
    /// 小さくなる処理
    /// </summary>
    public virtual void Contraction()
    {

    }

    /// <summary>
    /// 分裂する処理
    /// </summary>
    public virtual void Division()
    {

    }

    /// <summary>
    /// 爆発する処理
    /// </summary>
    public virtual void Explosion()
    {

    }
    
    public virtual void OnCollisionEnter(Collision other)
    {

    }
}
