using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 方向転換を行うクラス
/// </summary>
public class ChangeDirection : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    bool isChange = false;
    //[SerializeField, Header("レイの距離")]
    float maxDistance = 1;

    Enemy enemy;

    [SerializeField]
    Vector3 centerPosition = Vector3.zero;

    bool isReverse = false;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        if (transform.name.Contains("Cancer"))
        {
            isReverse = true;
        }
    }

    /// <summary>
    /// レイの長さ設定
    /// </summary>
    /// <returns></returns>
    float GetDistance()
    {
        float distance;

        if (transform.localScale.x >= 2.5f) distance = 4;
        else if (transform.localScale.x >= 1.5f) distance = 3;
        else distance = 1;

        return distance;
    }

    private void OnDrawGizmosSelected()
    {
        if (EditorApplication.isPlaying)
        {
            Gizmos.color = Color.red;
            if (enemy.direction_Left)
            {
                if(!isReverse)
                    Gizmos.DrawLine(transform.position + centerPosition, transform.position + (Vector3.down + Vector3.left) * GetDistance());
                else
                    Gizmos.DrawLine(transform.position + centerPosition, transform.position + (Vector3.down + Vector3.right) * GetDistance());
            }
            else
            {
                if(!isReverse)
                    Gizmos.DrawLine(transform.position + centerPosition, transform.position + (Vector3.down + Vector3.right) * GetDistance());
                else
                    Gizmos.DrawLine(transform.position + centerPosition, transform.position + (Vector3.down + Vector3.left) * GetDistance());
            }
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
            isChange = !Physics.Raycast(ray, out hit, maxDistance * GetDistance());

            if (isChange)
            {
                enemy.direction_Left = !enemy.direction_Left;
            }
        }
    }
}
