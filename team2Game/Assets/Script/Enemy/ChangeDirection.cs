using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 左右移動の方向転換を行うクラス
/// </summary>
public class ChangeDirection : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    bool isChange = false;//反転させるか

    public float MaxDistance
    {
        private get;
        set;
    }

    Enemy enemy;

    [SerializeField, Header("中心値")]
    Vector3 centerPosition = Vector3.zero;

    bool isReverse = false;//反転しているモデルか
    bool inWater = false;//水中の中か

    Vector3 leftRayDirection = Vector3.down + Vector3.left;
    Vector3 rightRayDirection = Vector3.down + Vector3.right;

    Vector3 transformCenter;//centerPositionを加えた最終的な中心値

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        if (transform.name.Contains("Cancer"))
        {
            isReverse = true;
        }
    }

    private void Update()
    {
        transformCenter = transform.position + centerPosition;
    }

    private void OnDrawGizmosSelected()
    {
        //プレイ中の表示
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.red;
        if (enemy.direction_Left)//左向き
        {
            if (!isReverse)//反転していない
                Gizmos.DrawLine(transformCenter, transformCenter + leftRayDirection);
            else//反転している
                Gizmos.DrawLine(transformCenter, transformCenter + rightRayDirection);
        }
        else//右向き
        {
            if (!isReverse)//反転していない
                Gizmos.DrawLine(transformCenter, transformCenter + rightRayDirection);
            else//反転している
                Gizmos.DrawLine(transformCenter, transformCenter + leftRayDirection);
        }
    }

    void DirectionChange()
    {
        if (enemy.NowChase) return;

        if (enemy.direction_Left)
        {
            if (!isReverse)
                ray = new Ray(transformCenter, leftRayDirection);
            else
                ray = new Ray(transformCenter, rightRayDirection);
        }
        else
        {
            if (!isReverse)
                ray = new Ray(transformCenter, rightRayDirection);
            else
                ray = new Ray(transformCenter, leftRayDirection);
        }

        //レイがオブジェクトに当たらなくなったら方向反転
        isChange = !Physics.Raycast(ray, out hit, MaxDistance);

        if (isChange || (hit.transform.tag != "Stage" && hit.transform.tag != "Ice"))
        {
            enemy.direction_Left = !enemy.direction_Left;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Stage" || collision.gameObject.tag == "Ice")
        {
            DirectionChange();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Contains("Water"))
        {
            inWater = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //敵に当たるか、水中でステージタグに当たった場合反転
        if(collision.gameObject.tag.Contains("Enemy")
            || (collision.gameObject.tag == "Stage" && inWater)
            || collision.gameObject.name.Contains("Wall"))
        {
            enemy.direction_Left = !enemy.direction_Left;
        }
    }
}
