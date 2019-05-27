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
    
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        sabunPos = playerPos.transform.position - transform.position;    //プレイヤーの位置－自分の位置
        sabunVec = sabunPos.normalized;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0,0,20*sabunVec.z));
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
            Destroy(this.gameObject);
        }
    }
}
