using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
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
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position+centerPosition, transform.position+(Vector3.down+Vector3.left)*GetDistance());
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Stage"))
        {
            if (enemy.direction_Left)
            {
                ray = new Ray(transform.position + centerPosition, Vector3.down + Vector3.left);
            }
            else
            {
                ray = new Ray(transform.position + centerPosition, Vector3.down + Vector3.right);
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
