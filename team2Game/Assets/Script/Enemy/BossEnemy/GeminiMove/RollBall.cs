using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollBall : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPos;　　　　//プレイヤーの位置
    private Vector3 sabunPos;　　　　　　//差分計算用
    private Vector3 sabunVec;　　　　　　//プレイヤーがいる方向を取得
    [SerializeField]
    private float speed = 10;　　　　　  //ボールスピード  変動可

    private Rigidbody rigid;
    [SerializeField]
    private int bgmNumber = 0;

    float rotate = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        sabunPos = playerPos.transform.position - transform.position;    //プレイヤーの位置－自分の位置
        sabunVec = sabunPos.normalized;
    }

    void Update()
    {
        rotate += 10;
        transform.Rotate(0,0,rotate * sabunVec.z);
        rigid.velocity = new Vector3(sabunVec.x * speed, rigid.velocity.y);
    }
    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision col)
    {
        //プレイヤーに当たったら死ぬ
        if (col.gameObject.tag == "Player")
        {
            PlayerManager.PlayerDamage(10);
            SoundManager.PlaySE(bgmNumber);
            Destroy(gameObject);
        }
        if (col.gameObject != gameObject && col.gameObject.tag != "Stage")
        {
            SoundManager.PlaySE(bgmNumber);
            Destroy(gameObject);
        }
    }
}
