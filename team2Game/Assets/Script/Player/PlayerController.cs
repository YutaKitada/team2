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

    private bool inWater;

    private float jumpTimer;

    private int jumpCount;

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody取得
        rigid = GetComponent<Rigidbody>();
        //最初は動ける
        isMoveStop = false;

        isJump = false;

        inWater = false;
        jumpTimer = 0;
        jumpCount = 0;
    }

    void FixedUpdate()
    {
        //移動制限がかかっていない場合
        if (!PlayerManager.isStop)
        {
            if (!inWater)
            {
                //重力を追加
                rigid.AddForce(Vector3.down * addGravity);
            }
            


            //ジャンプ処理
            Jump();
        }

        if (isJump)
        {
            jumpTimer += Time.deltaTime;
            if(jumpTimer >= 3f)
            {
                isJump = false;
                jumpCount = 0;
                jumpTimer = 0;
            }
        }

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
                //Playerの向きに応じて跳ね返る方向を変える
                switch (PlayerManager.playerDirection)
                {
                    //左を向いている場合
                    case PlayerManager.PlayerDirection.LEFT:
                        rigid.AddForce(new Vector3(5, 5), ForceMode.Impulse);
                        break;
                    //右を向いている場合
                    case PlayerManager.PlayerDirection.RIGHT:
                        rigid.AddForce(new Vector3(-5, 5), ForceMode.Impulse);
                        break;
                }
                //Rigidbodyの重力処理を開始
                rigid.useGravity = true;
            }
            //移動ができる状態であれば
            if (!isMoveStop)
            {
                //velocityに移動量を追加
                rigid.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * speed, rigid.velocity.y);

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
            //動けなくする
            isMoveStop = true;

        }
    }

    //ジャンプ処理メソッド
    private void Jump()
    {
        //ジャンプボタンを押したら
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            if (Input.GetAxisRaw("Vertical") <= -0.7f)
            {
                //上方向に力を与える
                rigid.AddForce(new Vector3(0, jumpPower * 1.5f), ForceMode.Impulse);

            }
            else
            {
                //上方向に力を与える
                rigid.AddForce(new Vector3(0, jumpPower), ForceMode.Impulse);
            }
            jumpCount++;
            if(jumpCount >= 2)
            {
                isJump = true;
            }
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

        if (collision.transform.tag == "Enemy")
        {
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

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        jumpCount = 0;
        if (isJump)
        {
            isJump = false;
        }
        if (other.tag == "Water")
        {
            speed = 3;
            jumpPower = 5f;
            inWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
        {
            speed = 10;
            jumpPower = 10;
            inWater = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isMoveStop && !PlayerManager.isStop)
        {
            //動けるようにする
            isMoveStop = false;
        }
        if(other.tag == "Water")
        {
            isJump = false;
        }
    }
}
