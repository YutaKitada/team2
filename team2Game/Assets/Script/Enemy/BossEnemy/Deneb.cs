using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deneb : BossEnemy
{
    [SerializeField, Header("通る曲線の座標")]
    Vector3[] vector;

    public bool OnRight
    {
        get;
        set;
    }

    bool isMove;//移動中か
    bool isWait;//待機中か

    float speed = 1;//動きや経過時間の速さ
    int maxHp;//最大体力

    [SerializeField]
    float interval = 3;
    float intervalElapsedTime;

    float t = 0;//ベジェ曲線移動の割合

    [SerializeField,　Header("降らせる星のオブジェ")]
    GameObject fallStar;
    [SerializeField, Header("降らせる星の座標の中心")]
    Vector3 fallPosition;
    [SerializeField, Header("横からくる星のオブジェ")]
    GameObject shootinStar;
    [SerializeField, Header("横からくる星の生成位置の高さ")]
    float shootPositionY;

    [SerializeField, Header("生成までの時間")]
    float instantInterval = 5;
    float instantElapsedTime = 0;

    MeshRenderer meshRenderer;

    public enum Mode//状態
    {
        NORMAL,
        MOVE
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
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead)
        {
            meshRenderer.enabled = true;
            Stop();

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
                Stop();
                StartCoroutine(DirectionCoroutine());
                Wait();
                break;

            case Mode.MOVE:
                StartCoroutine(MoveCoroutine());
                break;

            default:
                break;
        }

        FallStarGenerate();
        NowInvincible();
        Death();
    }

    public override void Damage()
    {
        if (!isHit) return;

        base.Damage();
    }

    /// <summary>
    /// 待機処理
    /// </summary>
    void Wait()
    {
        if (!isWait) return;

        intervalElapsedTime += Time.deltaTime * speed;
        if (intervalElapsedTime >= interval)
        {
            isMove = true;
            intervalElapsedTime = 0;
            mode = Mode.MOVE;
        }
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
    /// 降らせる星を生成
    /// </summary>
    void FallStarGenerate()
    {
        instantElapsedTime += Time.deltaTime * speed;
        if (instantElapsedTime <= instantInterval) return;

        //3つ生成してポジションを分ける
        for(int i=-1; i<2; i++)
        {
            Instantiate(fallStar, fallPosition + new Vector3(i * 6, Random.Range(-3f, 3f), 0), Quaternion.identity);
        }
        if (hp <= maxHp / 2)
        {
            Instantiate(shootinStar, GetShootPosition(), Quaternion.identity);
        }
        instantElapsedTime = 0;
    }

    /// <summary>
    /// 横からくる星の生成位置設定
    /// </summary>
    /// <returns></returns>
    Vector3 GetShootPosition()
    {
        Vector3 position;
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
        isMove = true;
        while (true)
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

        mode = Mode.NORMAL;
        OnRight = !OnRight;
        isMove = false;

        yield return null;
    }

    IEnumerator DirectionCoroutine()
    {
        float rate = 0;
        isWait = false;

        while(true)
        {
            //右側にいたら左側を、左側にいたら右側を向く
            rate += Time.deltaTime * 3 * speed;
            if (OnRight)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(forward), rate);
            }
            if (!OnRight)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-forward), rate);
            }

            //向き終わったら、isWaitをtrueに
            if(transform.rotation == Quaternion.Euler(forward) 
                || transform.rotation == Quaternion.Euler(-forward))
            {

                isWait = true;
                break;
            }
            else
            {
                yield break;
            }
        }

        yield return null;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            PlayerManager.PlayerDamage(10);
        }

        if (collision.gameObject.tag.Contains("Star"))
        {
            Damage();
        }
    }
}
