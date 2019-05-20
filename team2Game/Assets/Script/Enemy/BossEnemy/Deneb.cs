using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ラスボス：デネブ
/// </summary>
public class Deneb : BossEnemy
{
    [SerializeField, Header("通る曲線の座標")]
    Vector3[] vector;//ベジェ曲線の通る座標の配列

    float t = 0;//ベジェ曲線移動の割合

    /// <summary>
    /// 左右のどちらにいるか：true=右、false=左
    /// </summary>
    public bool OnRight
    {
        get;
        set;
    }

    bool isMove;//移動中か
    bool isWait;//待機中か

    float speed = 1;//動きや経過時間の速さ
    int maxHp;//最大体力

    // 体力取得用プロパティ
    public int Hp
    {
        get { return hp; }
    }

    [SerializeField]
    float interval = 3;//待機状態から攻撃モードへの移行までの時間
    float intervalElapsedTime;//上記に関する経過時間

    [SerializeField,　Header("降らせる星のオブジェ")]
    GameObject fallingStar;
    [SerializeField, Header("降らせる星の座標の中心")]
    Vector3 fallPosition;
    [SerializeField, Header("横からくる星のオブジェ")]
    GameObject shootingStar;
    [SerializeField, Header("横からくる星の生成位置の高さ")]
    float shootPositionY;

    [SerializeField, Header("生成までの時間")]
    float instantInterval = 5;
    float instantElapsedTime = 0;//上記に関する経過時間

    MeshRenderer meshRenderer;

    GameObject[] starArray;//上から生成した星を格納する配列
    GameObject star;//横から生成した星を格納する変数

    bool isNone = true;//starArray、starが存在していないか
    bool isShoot = false;//横から星が飛んできたか
    bool isInstante = false;//攻撃モード中に星を上に生成したか

    public enum Mode//状態
    {
        NORMAL,
        MOVE,
        ATTACK_STAR
    }
    Mode mode;//現在の状態

    // Start is called before the first frame update
    void Start()
    {
        transform.position = vector[0];

        rigid = GetComponent<Rigidbody>();
        rotation = Quaternion.identity;
        mode = Mode.NORMAL;

        maxHp = hp;

        OnRight = true;
        isMove = false;
        isWait = true;

        meshRenderer = GetComponent<MeshRenderer>();

        starArray = new GameObject[3];
    }

    // Update is called once per frame
    void Update()
    {
        Stop();

        if (IsDead)
        {
            meshRenderer.enabled = true;

            //死亡してから、アニメーションが終わるまでのおおよその時間経過でパーティクル生成、
            //かつ、return以下の処理を行わない
            deadElapsedTime += Time.deltaTime;
            if (deadElapsedTime >= 2)
            {
                Instantiate(downParticle, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            return;
        }

        //体力が半分を切ったら、2倍の速度に変化
        if (hp <= maxHp / 2)
        {
            speed = 2;
        }

        switch (mode)
        {
            case Mode.NORMAL:
                StartCoroutine(DirectionCoroutine());
                break;

            case Mode.MOVE:
                StartCoroutine(MoveCoroutine());
                break;

            case Mode.ATTACK_STAR:
                StarAttack();
                Mode_MoveChange();
                break;

            default:
                break;
        }
        
        NowInvincible();
        Death();
    }

    /// <summary>
    /// 無敵時間の処理
    /// </summary>
    void NowInvincible()
    {
        if (isHit || IsDead) return;

        invincibleElapsedTime += Time.deltaTime * speed;
        meshRenderer.enabled = !meshRenderer.enabled;

        if (invincibleElapsedTime >= invincibleTime)
        {
            invincibleElapsedTime = 0;
            isHit = true;
            meshRenderer.enabled = true;
        }
    }

    /// <summary>
    /// 横からくる星の生成位置設定
    /// </summary>
    /// <returns></returns>
    Vector3 GetShootPosition()
    {
        Vector3 position;
        //x座標は自身に合わせ、y座標はinspectorで調整
        if(OnRight)
        {
            position = new Vector3(vector[0].x, shootPositionY);
        }
        else
        {
            position = new Vector3(vector[3].x, shootPositionY);
        }

        return position;
    }

    /// <summary>
    /// 移動の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveCoroutine()
    {
        //星が生成されていれば動かない
        if (!GetStarObjects()) yield break;

        while (isMove)
        {
            if (OnRight)
            {
                transform.position = Bezier.GetPoint(vector[0], vector[1], vector[2], vector[3], t);

                //割合を増やす
                t += 0.01f * speed;
                if (t > 1.0f) t = 0;

                //終点との差が0.1f以下になるまで実行
                if (Mathf.Abs(vector[3].x - transform.position.x) >= Mathf.Abs(0.1f))
                {
                    yield break;
                }
                else
                    break;
            }
            else
            {
                transform.position = Bezier.GetPoint(vector[3], vector[2], vector[1], vector[0], t);

                //割合を増やす
                t += 0.01f * speed;
                if (t > 1.0f) t = 0;

                //終点との差が0.1f以下になるまで実行
                if (Mathf.Abs(vector[0].x - transform.position.x) >= Mathf.Abs(0.1f))
                {
                    yield break;
                }
                else
                    break;
            }
        }

        //左右の位置のboolを変更
        //待機状態に移行
        OnRight = !OnRight;
        isMove = false;
        isWait = true;
        mode = Mode.NORMAL;
        yield return null;
    }

    /// <summary>
    /// 方向転換
    /// </summary>
    /// <returns></returns>
    IEnumerator DirectionCoroutine()
    {
        float rate = 0;

        while(isWait)
        {
            //右側にいたら左側を、左側にいたら右側を向く
            rate += Time.deltaTime * 3 * speed;
            if (OnRight)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(forward), rate);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-forward), rate);
            }
            break;
        }

        //方向転換し終えたら、モード移行までの待機時間
        if (GetAngle())
        {
            //指定した経過時間を過ぎたらモードを移行
            intervalElapsedTime += Time.deltaTime * speed;
            if (intervalElapsedTime >= interval)
            {
                intervalElapsedTime = 0;
                mode = Mode.ATTACK_STAR;
                yield return null;
            }
            //過ぎなければ中断
            else
            {
                yield break;
            }
        }
    }

    bool GetAngle()
    {
        bool isForward;

        if(transform.rotation==Quaternion.Euler(new Vector3(0,-90,0))
            || transform.rotation==Quaternion.Euler(new Vector3(0,90,0))
            || transform.rotation == Quaternion.Euler(new Vector3(0, 270, 0))
            || transform.rotation == Quaternion.Euler(new Vector3(0, -270, 0)))
        {
            isForward = true;
            return isForward;
        }

        isForward = false;
        return isForward;
    }

    /// <summary>
    /// 星を使った攻撃
    /// </summary>
    void StarAttack()
    {
        instantElapsedTime += Time.deltaTime * speed;
        if (instantElapsedTime >= instantInterval)
        {
            if (GetStarObjects() && !isInstante)
            {
                //3つ生成してポジションを分ける
                for (int i = -1; i < 2; i++)
                {
                    //0から順に格納
                    starArray[i + 1] = Instantiate(fallingStar, fallPosition + new Vector3(i * 6, Random.Range(-3f, 3f), 0), Quaternion.Euler(0, 90, 90));
                }
                isInstante = true;
            }

            //体力が半分以下のとき
            if (hp <= maxHp / 2)
            {
                //生成した星が全て消えた時1つだけ生成
                if (GetStarObjects() && !isShoot)
                {
                    star = Instantiate(shootingStar, GetShootPosition(), Quaternion.Euler(0, 90, 90));
                    isShoot = true;
                }
            }
        }
    }

    bool GetStarObjects()
    {
        //生成した3つの星のうち1つでも存在すればfalse、
        //なければtrue
        for(int i = 0; i < 3; i++)
        {
            if (starArray[i] != null)
            {
                isNone = false;
                return isNone;
            }
        }

        isNone = true;
        return isNone;
    }

    /// <summary>
    /// 攻撃モードから移動モードへの移行
    /// </summary>
    void Mode_MoveChange()
    {
        //現体力に応じて切り替えるタイミングを調整

        //最大体力の半分以下なら
        if (hp <= maxHp / 2)
        {
            //横から生成した星が無くなったとき
            if (GetStarObjects() && (star == null && isShoot))
            {
                instantElapsedTime = 0;
                isMove = true;
                isWait = false;
                isShoot = false;
                isInstante = false;
                mode = Mode.MOVE;
            }
        }
        //最大体力の半分よりも多ければ
        else
        {
            //上から生成したすべての星が無くなったとき
            if (GetStarObjects() && isInstante)
            {
                instantElapsedTime = 0;
                isMove = true;
                isWait = false;
                isInstante = false;
                mode = Mode.MOVE;
            }
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            PlayerManager.PlayerDamage(10);
        }

        if (collision.gameObject.tag.Contains("Star"))
        {
            Damage(1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        for(int i=0; i<4; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(vector[i], 1);
        }
    }
}
