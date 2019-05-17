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

    private Animator animator;                  //アニメーション用

    private int jumpCount;                      //2段ジャンプ用カウンター

    private bool isIce;                         //Iceに当たっているかどうか//追加丹下

    private bool isDamage;                      //ダメージを受けているか否か

    [HideInInspector]
    public bool gravityArea;//重力反転エリアに入っているかどうか
    private bool gravityStop;//重力無効判定用

    [HideInInspector]
    public bool normalRotation;

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

        //ダメージを受けていない
        isDamage = false;

        //アニメーション用
        animator = GetComponent<Animator>();

        gravityArea = false;//重力反転エリアに入っていない
        gravityStop = false;

        normalRotation = true;
    }

    void FixedUpdate()
    {
        //移動制限がかかっていない場合
        if (!PlayerManager.isStop && !isMoveStop && !isDamage)
        {
            if (!gravityArea) rigid.AddForce(new Vector3(0, -addGravity));
            else rigid.AddForce(new Vector3(0, addGravity));
            Gravity();

            //ジャンプ処理
            Jump();
        }

        //アニメーションの処理
        Animation();

        //移動処理
        Move();

        UIManager.debugtext = "\n" + "JumpCount:" + jumpCount
             + "\n" + "isJump:" + isJump;
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
                if (isIce)
                {
                    rigid.AddForce(Input.GetAxisRaw("Horizontal") * speed, 0, 0);
                }
                else
                {
                    //velocityに移動量を追加
                    rigid.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * speed, rigid.velocity.y);
                    //rigid.AddForce(new Vector3(Input.GetAxisRaw("Horizontal") * speed, 0));
                }

                //左に移動していれば
                if (rigid.velocity.x < 0)
                {
                    //Playerの向きを左に
                    PlayerManager.playerDirection = PlayerManager.PlayerDirection.LEFT;
                    

                }
                //右に移動していれば
                if (rigid.velocity.x > 0)
                {
                    //Playerの向きを右に
                    PlayerManager.playerDirection = PlayerManager.PlayerDirection.RIGHT;
                    

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

            if (PlayerManager.isWishMode)
            {
                transform.LookAt(transform.position + new Vector3(0, 0, -1));
            }
        }

        if(PlayerManager.playerDirection == PlayerManager.PlayerDirection.LEFT)
        {
            transform.LookAt(transform.position + new Vector3(-1, 0));
            if (!normalRotation)
            {
                transform.LookAt(transform.position + new Vector3(1, 0));
                transform.Rotate(new Vector3(180, 0));
            }
        }
        else if(PlayerManager.playerDirection == PlayerManager.PlayerDirection.RIGHT)
        {
            transform.LookAt(transform.position + new Vector3(1, 0));
            if (!normalRotation)
            {
                transform.LookAt(transform.position + new Vector3(-1, 0));
                transform.Rotate(new Vector3(180, 0));
            }
        }

        if (gravityArea)
        {
            if(rigid.velocity.y >= 5)
            {
                normalRotation = false;
            }
        }
        else
        {
                if (rigid.velocity.y <= -5)
                {
                    normalRotation = true;
                }
            
        }
    }

    //ジャンプ処理メソッド
    private void Jump()
    {
        //ジャンプボタンを押したら
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            //ジャンプする前に一旦移動量を0にする
            rigid.velocity = Vector3.zero;
            if (Input.GetAxisRaw("Vertical") <= -0.7f)
            {

                SoundManager.PlaySE(4);
                if (!gravityArea)
                {
                    //上方向に力を与える
                    rigid.AddForce(new Vector3(0, jumpPower * 1.5f), ForceMode.Impulse);
                }
                else
                {
                    //上方向に力を与える
                    rigid.AddForce(new Vector3(0, -jumpPower * 1.5f), ForceMode.Impulse);
                }
                //ジャンプカウントを1増やす
                jumpCount++;
            }
            else
            {
                if (!gravityArea)
                {
                    //上方向に力を与える
                    rigid.AddForce(new Vector3(0, jumpPower), ForceMode.Impulse);
                }
                else
                {
                    //上方向に力を与える
                    rigid.AddForce(new Vector3(0, -jumpPower), ForceMode.Impulse);
                }
                SoundManager.PlaySE(6);
                //ジャンプカウントを1増やす
                jumpCount++;
            }
            
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
        //願い事を適えている時以外
        if (!PlayerManager.isWishMode)
        {
            //横方向の入力があれば走るアニメーション
            if (Input.GetAxisRaw("Horizontal") >= 0.8f || Input.GetAxisRaw("Horizontal") <= -0.8f)
            {
                animator.SetBool("Run", true);
            }
            else
            {
                animator.SetBool("Run", false);
            }

            //下方向の入力があれば座る
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

            //ジャンプアニメーション
            if (Input.GetButtonDown("Jump"))
            {
                animator.SetTrigger("Jump");
            }
        }
        if (PlayerManager.isWishMode)
        {
            if (!animator.GetBool("Squat"))
            {
                animator.SetBool("Run", false);
                animator.SetBool("Squat", true);
            }
        }

    }

    private void Gravity()
    {

        if (gravityArea)
        {
            if (!gravityStop)
            {
                //rigid.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * speed, rigid.velocity.y);
                rigid.AddForce(new Vector3(0, 9.8f));
            }
            //else
            //{

            //    rigid.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * speed, 9.8f);
            //}
        }
        //else Move();
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
            //ダメージをまだ受けていない場合
            if (!isDamage)
            {
                //ダメージを受けた時の効果音の再生
                SoundManager.PlaySE(5);
                //ダメージを受けた
                isDamage = true;

                //一旦移動量を0に
                rigid.velocity = Vector3.zero;

                //当たった場所は自身の右か左かを取得
                Vector3 hitVector = (collision.transform.position - transform.position).normalized;
                //右であれば
                if (hitVector.x >= 0)
                {
                    rigid.AddForce(new Vector3(-1, 3), ForceMode.Impulse);
                }
                //左であれば
                else
                {
                    rigid.AddForce(new Vector3(1, 3), ForceMode.Impulse);
                }
            }

        }
        rigid.useGravity = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "UpStage")
        {
            if (gravityArea)
            {
                gravityStop = true;
                jumpCount = 0;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.transform.tag == "UpStage")
        {
            gravityStop = false;
        }
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
            isJump = false;
        }

        animator.SetTrigger("Landing");

        if (other.tag == "Stage")
        {
            isDamage = false;
            //ジャンプカウントを0に
            jumpCount = 0;
            //isJumpがtrueの場合、falseに
            if (isJump) isJump = false;
        }

        if (other.tag == "Gravity")
        {
            gravityArea = true;
            rigid.useGravity = false;
            //transform.Rotate(new Vector3(180, 0));
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
        if (other.tag == "Gravity")
        {
            gravityArea = false;
            //GravityStop = false;
            rigid.useGravity = true;
            
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
        if (other.tag == "Water")
        {
            isJump = false;     //ジャンプの制限をなくす
            isDamage = false;
            jumpCount = 0;
        }
    }
}
