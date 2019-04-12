using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    NORMAL,
    CHASE
}

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
    
    protected Transform target;

    protected Vector3 distance;//targetとの距離

    protected Vector3 forward = new Vector3(0, -90, 0);//正面

    [SerializeField]
    protected int hp = 1;

    protected State state;

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
    /// 倒したかどうか
    /// </summary>
    public bool Defeat
    {
        protected set;
        get;
    } = false;

    /// <summary>
    /// 移動処理
    /// </summary>
    public virtual void Move()
    {

    }

    /// <summary>
    /// 方向の設定
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
        hp--;
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

    /// <summary>
    /// ターゲット設定
    /// </summary>
    public virtual void SetTarget()
    {

    }
    
    public virtual void OnCollisionEnter(Collision other)
    {

    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    public virtual void Death()
    {
        if(hp <= 0)
        {
            EnemyManager.DefeatedCount++;
            Debug.Log("倒した数：" + EnemyManager.DefeatedCount);
            Defeat = true;
            Destroy(gameObject);
        }
    }
}
