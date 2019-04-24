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

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        if (transform.name.Contains("Cancer"))
        {
            isReverse = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.red;
        if (enemy.direction_Left)
        {
            if (!isReverse)
                Gizmos.DrawLine(transform.position + centerPosition, transform.position + (Vector3.down + Vector3.left));
            else
                Gizmos.DrawLine(transform.position + centerPosition, transform.position + (Vector3.down + Vector3.right));
        }
        else
        {
            if (!isReverse)
                Gizmos.DrawLine(transform.position + centerPosition, transform.position + (Vector3.down + Vector3.right));
            else
                Gizmos.DrawLine(transform.position + centerPosition, transform.position + (Vector3.down + Vector3.left));
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Stage"))
        {
            if (enemy.direction_Left)
            {
                if(!isReverse)
                    ray = new Ray(transform.position + centerPosition, Vector3.down + Vector3.left);
                else
                    ray = new Ray(transform.position + centerPosition, Vector3.down + Vector3.right);
            }
            else
            {
                if(!isReverse)
                    ray = new Ray(transform.position + centerPosition, Vector3.down + Vector3.right);
                else
                    ray = new Ray(transform.position + centerPosition, Vector3.down + Vector3.left);
            }

            //レイがオブジェクトに当たらなくなったら方向反転
            isChange = !Physics.Raycast(ray, out hit, MaxDistance);

            if (isChange)
            {
                enemy.direction_Left = !enemy.direction_Left;
            }
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
            || (collision.gameObject.tag.Contains("Stage") && inWater))
        {
            enemy.direction_Left = !enemy.direction_Left;
        }
    }
}
