using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMovement : MonoBehaviour
{

    private Rigidbody rigid;            //移動用Rigidbody

    [HideInInspector]
    public bool returnPlayer;           //地面に触れて、Playerのもとへ帰ってきているか否か

    [SerializeField]
    private float returnPower = 10;     //Playerの元へ帰る際の強さ

    private float returnX;              //跳ねる際の横方向の強さの格納用

    private bool inWater;               //水の中にいるか否か

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        returnPlayer = false;
        
        returnX = 0;
        inWater = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("STAR:"+returnPlayer);

        ////跳ねずにそのまま帰ってくる際の処理
        //if (returnPlayer)
        //{
        //    Vector3 returnVector = GameManager.player.transform.position - transform.position;

        //    returnVector = returnVector.normalized;

        //    //rigid.AddForce(returnVector * returnPower);
        //    //rigid.velocity = returnVector * returnPower;
        //}
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //まだ地面に触れていなければ
        if (!returnPlayer && gameObject.layer == 12)
        {
            returnPlayer = true;                    //地面に触れ、Playerの元へ帰る
            PlayerManager.isWishMode = false;       //願い事モードを切る
            gameObject.layer = 11;                  //星のLayerを変更(Playerでも触れられるものへ)

            //当たったのがEnemyであれば
            if (collision.transform.tag == "Enemy")
            {
                //温度ゲージが100以下であれば
                if (UIManager.hpGageFillAmount <= 100)
                {
                    //コンボが続いていれば
                    if (UIManager.isCombo)
                    {
                        //ゲージ回復10に加えて、コンボ数に応じて1.2倍の回復量を得る
                        UIManager.hpGageFillAmount += 10f * (1 + (float)GameManager.combo * 0.2f) ;
                    }
                    else
                    {
                        //ゲージを10回復
                        UIManager.hpGageFillAmount += 10f;
                    }
                }
                //温度ゲージが100以上であれば
                else
                {
                    //ゲージを0.5回復
                    UIManager.hpGageFillAmount += 0.5f;
                }
                //コンボ中でなければ
                if (!UIManager.isCombo)
                {
                    UIManager.isCombo = true;   //コンボを開始
                }
                GameManager.combo++;        //コンボ数を1増やす

                //collision.gameObject.GetComponent<Enemy>().
            }
            //当たったのがEnemyでもPlayerでもなければ
            else if (collision.transform.tag != "Player")
            {
                //温度ゲージを5減らす
                UIManager.hpGageFillAmount -= 5f;

                //コンボ中であれば
                if (UIManager.isCombo)
                {
                    GameManager.combo = 0;          //コンボ数を0に
                    UIManager.isCombo = false;      //コンボ中ではなくなる
                }
            }

            //もし水の中でなければ
            if (!inWater)
            {
                rigid.velocity = Vector3.zero;      //一旦移動量を0に

                Vector3 vector = GameManager.player.transform.position - transform.position;　　  //Playerのいる方向へのベクトルを取得

                //ベクトルのxの値に応じて戻る方向を決める
                if (vector.x > 0)       //ベクトルが右を向いていれば
                {
                    returnX = 5 + GameManager.combo;        //returnXの値を5に、それに加えてコンボ数を足す
                }
                else if (vector.x < 0)
                {
                    returnX = -5 - GameManager.combo;       //returnXの値を-5に、それに加えてコンボ数を足す
                }

                rigid.AddForce(new Vector3(returnX, 8), ForceMode.Impulse);     //Playerのいる方向に跳ねる
            }

            UIManager.hpGageStopTimer = 0;        //温度ゲージの減少を止めるタイマーの初期化
            UIManager.comboGageStopTimer = 0;
            PlayerThrow.dank = false;
        }

        //Playerに触れたら
        if (collision.transform.tag == "Player")
        {
            returnPlayer = false;       //Playerの元へ帰ってきたのでfalseに
            inWater = false;            //水の中ではない判定
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //水に入ったら
        if(other.tag == "Water")
        {
            inWater = true;         //水に入った
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //水から出たら
        if (other.tag == "Water")
        {
            inWater = false;        //水から出た
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //水に触れていたら
        if(other.tag == "Water")
        {
            rigid.AddForce(new Vector3(0, 15));     //上方向に力をいれる
        }
    }
}
