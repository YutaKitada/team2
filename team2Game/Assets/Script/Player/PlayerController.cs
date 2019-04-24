using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 10;                   //Playerの移動速度
    [SerializeField]
    private float jumpPower = 10;               //Playerのジャンプする力
    [SerializeField]
    private float addGravity = 10;              //Playerの受ける重力

    private Rigidbody rigid;                    //Player用Rigidbody

    private Vector3 lastVelocity;               //停止処理前のvelocity
    private bool isMoveStop;                    //Trueであれば動けない

    private bool isJump;                        //ジャンプフラッグ

    private bool inWater;                       //水の中にいるか否か

    private float jumpTimer;                    //jumpがもういちどできるまでのタイマー

    private Animator animator;

    private int jumpCount;                      //2段ジャンプ用カウンター

    private bool isIce;                         //Iceに当たっているかどうか//追加丹下

    private bool isDamage;

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody取得
        rigid = GetComponent<Rigidbody>();
        //最初は動ける
        isMoveStop = false;
        //ジャンプ中ではない
        isJump = false;

        //水の中ではない
        inWater = false;

        //タイマー・カウントの初期化
        jumpTimer = 0;
        jumpCount = 0;

        //Iceに当たっていない//追加丹下
        isIce = false;

        isDamage = false;

        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //移動制限がかかっていない場合
        if (!PlayerManager.isStop && !isMoveStop && !isDamage)
        {
            //ジャンプ処理
            Jump();

            rigid.AddForce(new Vector3(0, -addGravity));
        }

        Animation();

        //移動処理
        Move();
    }

    //移動処理メソッド
    private void Move()
    {
        //移動処理がかかっていない場合
        if (!PlayerManager.isStop)
        {
            //useGravityがFalseであれば
            if (!rigid.useGravity)
            {
                //Rigidbodyの重力処理を開始
                rigid.useGravity = true;
            }
            //移動ができる状態であれば
            if (!isMoveStop && !isDamage)
            {
                //Iceに触れていれば//追加丹下
                if  (isIce)
                {
                    rigid.AddForce(Input.GetAxisRaw("Horizontal") * speed, rigid.velocity.y, 0);
                }
                else
                {
                    //velocityに移動量を追加
                    rigid.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * speed, rigid.velocity.y);
                }
                //左に移動していれば
                if (rigid.velocity.x < 0)
                {
                    //Playerの向きを左に
                    PlayerManager.playerDirection = PlayerManager.PlayerDirection.LEFT;
                    transform.LookAt(transform.position + new Vector3(-1, 0));

                }
                //右に移動していれば
                if (rigid.velocity.x > 0)
                {
                    //Playerの向きを右に
                    PlayerManager.playerDirection = PlayerManager.PlayerDirection.RIGHT;
                    transform.LookAt(transform.position + new Vector3(1, 0));

                }
            }

        }
        //移動処理がかかっている場合
        else if (PlayerManager.isStop && !isMoveStop)
        {
            //移動を止める
            rigid.velocity = Vector3.zero;
            //Rigidbodyの重力処理
            rigid.useGravity = false;
            ////動けなくする
            //isMoveStop = true;
        }
    }

    //ジャンプ処理メソッド
    private void Jump()
    {
        //ジャンプボタンを押したら
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            rigid.velocity = Vector3.zero;
                if (Input.GetAxisRaw("Vertical") <= -0.7f)
                {
                    //上方向に力を与える
                    rigid.AddForce(new Vector3(0, jumpPower * 1.5f), ForceMode.Impulse);
                    SoundManager.PlaySE(4);

                }
                else
                {
                    //上方向に力を与える
                    rigid.AddForce(new Vector3(0, jumpPower), ForceMode.Impulse);
                    SoundManager.PlaySE(6);
                }
                //ジャンプカウントを1増やす
                jumpCount++;
                //ジャンプカウントが2以上であれば
                if (jumpCount >= 2)
                {
                    //ジャンプをできなくする
                    isJump = true;
                }
        }
        

    }

    private void Animation()
    {
        if (Input.GetAxisRaw("Horizontal") >= 0.6f || Input.GetAxisRaw("Horizontal") <= -0.6f)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        if (Input.GetAxisRaw("Vertical") <= -0.7f)
        {
            if (!animator.GetBool("Squat"))
            {
                animator.SetBool("Squat", true);
            }
        }
        else
        {
            animator.SetBool("Squat", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("Jump");
        }
    }

        private void OnCollisionEnter(Collision collision)
    {
        ////ステージに当たった場合
        //if(collision.transform.tag == "Stage")
        //{
        //動けない状態であったら
        if (isMoveStop)
        {
            //動けるようにする
            isMoveStop = false;
        }

        //Enemyに当たった場合
        if (collision.transform.tag == "Enemy")
        {
            //ダメージを10受ける
            PlayerManager.PlayerDamage(10);

            //Vector3 direciton = (collision.transform.position - transform.position).normalized;

            //if (direciton.x > 0)
            //{
            //    rigid.AddForce(new Vector3(-10, 5), ForceMode.Impulse);
            //    isMoveStop = true;
            //}
            //else if (direciton.x < 0)
            //{
            //    rigid.AddForce(new Vector3(10, 5), ForceMode.Impulse);
            //    isMoveStop = true;
            //}
            if (!isDamage)
            {
                SoundManager.PlaySE(5);
                isDamage = true;

                rigid.velocity = Vector3.zero;

                Vector3 hitVector = (collision.transform.position - transform.position).normalized;
                if (hitVector.x >= 0)
                {
                    rigid.AddForce(new Vector3(-1, 3), ForceMode.Impulse);
                }
                else
                {
                    rigid.AddForce(new Vector3(1, 3), ForceMode.Impulse);
                }
            }

        }
        rigid.useGravity = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        rigid.useGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        //水の中に入ったら
        if (other.tag == "Water")
        {
            speed = 3;          //スピードを3に
            jumpPower = 5f;     //ジャンプのパワーを5に
            inWater = true;     //水の中である
        }

        //Iceに触れていたら//追加丹下
        if (other.tag == "Ice")
        {
            isIce = true;//Iceに触れている
        }

        animator.SetTrigger("Landing");

        if(other.tag == "Stage")
        {
            isDamage = false;
            //ジャンプカウントを0に
            jumpCount = 0;
            //isJumpがtrueの場合、falseに
            if (isJump) isJump = false;
        }

        
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
        {
            speed = 10;         //スピードを10に
            jumpPower = 10;     //ジャンプのパワーを10に
            inWater = false;    //水の中ではない
        }
        //追加丹下
        if (other.tag == "Ice")
        {
            isIce = false;//Iceに触れていない
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        //移動制限がかかっている場合
        if (isMoveStop && !PlayerManager.isStop)
        {
            //動けるようにする
            isMoveStop = false;
        }
        //水の中にいる場合
        if(other.tag == "Water")
        {
            isJump = false;     //ジャンプの制限をなくす
            isDamage = false;
        }
    }
}
