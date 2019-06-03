using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBallAttack : MonoBehaviour
{

    private Vector3 playerPos;　　　　　//プレイヤーの位置
    private float angle = 45.0f;　　　　//角度45固定
    private Vector3 offset;　　　　　　 //自分の位置
    private Vector3 sabunVec;　　　　　 //プレイヤーがいる方向を取得
    [SerializeField]
    private int bgmNumber = 0;　　　　　//音番号
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        SetTarget(playerPos, angle);
    }
    
    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
        {
            PlayerManager.PlayerDamage(5);
            SoundManager.PlaySE(bgmNumber);
            Destroy(gameObject);
        }
        else if(col.gameObject !=gameObject)
        {
            SoundManager.PlaySE(bgmNumber);
            Destroy(gameObject);
        }
       
    }

    void SetTarget(Vector3 playerPos, float angle)
    {
        this.playerPos = playerPos - transform.position;
        sabunVec = this.playerPos.normalized;
        offset = transform.position;
        this.angle = angle;

        StartCoroutine("ThrowBall");
    }

    IEnumerator ThrowBall()
    {
        float b = Mathf.Tan(angle * Mathf.Deg2Rad);
        //プレイヤーが右にいた時
        if (sabunVec.x >= 0)
        {
            float a = (playerPos.y - b * playerPos.x) / (playerPos.x * playerPos.x);
            for (float x = 0; x <= playerPos.x * 2; x += 0.3f)
            {
                float y = a * x * x + b * x;
                transform.position = new Vector3(x, y, 0) + offset;
                yield return null;
            }
        }
        //プレイヤーが左にいた時
        else
        {
            float a = (playerPos.y + b * playerPos.x) / (playerPos.x * playerPos.x);
            for (float x = 0; x >= playerPos.x * 2; x -= 0.3f)
            {
                float y = a * x * x + (b * x) * -1;
                transform.position = new Vector3(x, y, 0) + offset;
                yield return null;
            }
        }
    }

    

    
    
}
