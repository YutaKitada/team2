using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemyの大元
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public enum State
    {
        NORMAL,
        CHASE
    }

    protected Rigidbody rigid;
    protected Quaternion rotation;

    [SerializeField, Header("移動力")]
    protected float power = 10f;
    protected float maxSpeed = 0f;//最大移動スピード(Startメソッドで決定)
    
    protected Transform target;
    protected Vector3 distance;//targetとの距離
    [SerializeField, Header("Playerを追いかけるか")]
    protected bool isChase = false;

    protected Vector3 forward = new Vector3(0, -90, 0);//正面

    [SerializeField]
    protected int hp = 1;

    protected State state;

    [SerializeField]
    protected GameObject downParticle;

    public bool direction_Left = true;
    
    public bool direction_Up = true;

    /// <summary>
    /// 倒したかどうか
    /// </summary>
    public bool Defeat
    {
        get;
        protected set;
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
        //左側を正面にする
        if (direction_Left)
        {
            rotation = Quaternion.Euler(forward);
        }
        //右側を正面にする
        else
        {
            rotation = Quaternion.Euler(-forward);
        }
        //正面を進行方向にして移動
        transform.rotation = rotation;
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
            ParticleGenerate();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// パーティクル生成
    /// </summary>
    public void ParticleGenerate()
    {
        if (downParticle == null) return;

        Instantiate(downParticle, transform.position, Quaternion.identity);
    }
}
