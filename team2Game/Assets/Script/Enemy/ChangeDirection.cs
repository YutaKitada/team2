using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirection : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    bool isChange;
    [SerializeField, Header("レイの距離")]
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
        if (enemy.Direction_Left)
        {
            ray = new Ray(transform.position, Vector3.down + Vector3.left);
        }
        else
        {
            ray = new Ray(transform.position, Vector3.down + Vector3.right);
        }

        //レイがオブジェクトに当たらなくなったら方向反転
        isChange = !Physics.Raycast(ray, out hit, maxDistance);
        Debug.Log(isChange);

        if (isChange)
        {
            enemy.Direction_Left = !enemy.Direction_Left;
        }
    }
}
