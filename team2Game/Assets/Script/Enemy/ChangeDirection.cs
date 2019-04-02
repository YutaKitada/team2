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
    bool isChange;
    //[SerializeField, Header("レイの距離")]
    float maxDistance = 1;

    Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        ReverseDirection();
    }

    /// <summary>
    /// 方向反転
    /// </summary>
    void ReverseDirection()
    {
        if (enemy.Direction_Left)
        {
            ray = new Ray(transform.position, Vector3.down + Vector3.left);
        }
        else
        {
            ray = new Ray(transform.position, Vector3.down + Vector3.right);
        }

        //レイがオブジェクトに当たらなくなったら方向反転
        isChange = !Physics.Raycast(ray, out hit, maxDistance * GetDistance());

        if (isChange)
        {
            enemy.Direction_Left = !enemy.Direction_Left;
        }
    }

    float GetDistance()
    {
        float distance;
        if (transform.localScale.x <= 2) distance = 3;
        else distance = 1;

        return distance;
    }
}
