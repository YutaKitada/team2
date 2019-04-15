using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Libra : BossEnemy
{
    public enum Mode
    {
        NORMAL,
        INVINCIBLE
    }
    Mode mode;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        mode = Mode.NORMAL;
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case Mode.NORMAL:

                break;

            case Mode.INVINCIBLE:
                NowInvincible();
                break;

            default:
                break;
        }

        Death();
    }

    /// <summary>
    /// 無敵時間の処理
    /// </summary>
    void NowInvincible()
    {
        invincibleElapsedTime += Time.deltaTime;

        if (invincibleElapsedTime >= invincibleTime)
        {
            invincibleElapsedTime = 0;
            mode = Mode.NORMAL;
            isHit = true;
        }
    }

    public override void OnCollisionEnter(Collision other)
    {
        //Playerに当たったらPlayerにダメージ
        if (other.gameObject.tag.Contains("Player"))
        {
            PlayerManager.PlayerDamage(10);
            mode = Mode.NORMAL;
        }

        if (other.gameObject.tag.Contains("Star"))
        {
            Damage();
            isHit = false;
        }
    }
}
