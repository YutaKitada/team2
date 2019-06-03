using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helth : MonoBehaviour
{

    [SerializeField]
    private int hp =10;　　　     　　　　//体力


    [SerializeField]
    private GameObject hitParticl;　　　　//当たり位置のパーティクル
    Animator anime;
    [SerializeField]
    private Vector3 playerVec;　　　　　　//プレイヤーのノックバックベクトル
    private bool isHit;

    private float maxTime = 2;　　　　　　//最大待機時間
    private float currentTime;            //現在の時間
    
    private bool dead;                    //死亡判定
    public bool Dead
    {
        get
        {
            return dead;
        }
        private set
        {
            dead = value;
        }
    }

    [SerializeField]
    float intervalTime = 1;               //アニメーションを待つ時間
    float elapsedTime;
    [SerializeField]
    private int seNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
        dead = false;
        isHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        Death();
        //一回ずつ当たったら数秒無敵時間
        if(isHit)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= maxTime)
            {
                currentTime = 0;
                isHit = false;
            }
        }
        

    }
    private void OnCollisionEnter(Collision col)
    {
        //プレイヤーに当たったらプレイヤーにダメージを与える
        if (col.gameObject.tag == "Player")
        {
            col.transform.GetComponent<PlayerController>().Damage(playerVec);
        }
        //星に当たったら自分のhpを減らす。
        if (col.gameObject.tag == "Star"&&!isHit)
        {
            isHit = true;
            Instantiate(hitParticl, col.contacts[0].point, Quaternion.identity);
            
            if(hp <= 0)
            {
                return;
            }
            hp--;
            if(hp<=0)
            {
                dead = true;
                anime.SetTrigger("IsDead");
                SoundManager.PlaySE(seNumber);
            }
        }
    }
    
    public void Death()
    {
        //死んだらアニメーションが終わるのを待つ
        if(dead)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >=intervalTime)
            {
                BossClear.SubCount(1);
                Destroy(gameObject);
            }
        }
    }
}
