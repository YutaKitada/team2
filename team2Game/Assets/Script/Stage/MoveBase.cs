using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    UP,
    DOWN
}

public class MoveBase : MonoBehaviour
{
    [SerializeField, Header("移動方向")]
    Direction direction = Direction.UP;

    [SerializeField, Header("移動上限値")]
    float moveLimit = 2;

    float upperLimit;//上方向上限値
    float downerLimit;//下方向上限値

    float startPositionY;//Y座標の初期値

    [SerializeField, Header("停止時間")]
    float stopTime = 2;
    float elapsedTime = 0;//経過時間

    private void Start()
    {
        startPositionY = transform.position.y;

        upperLimit = startPositionY + moveLimit;
        downerLimit = startPositionY - moveLimit;
    }

    // Update is called once per frame
    void Update()
    {
        Move_Up();
        Move_Down();
    }

    /// <summary>
    /// 上移動
    /// </summary>
    void Move_Up()
    {
        if (direction != Direction.UP) return;

        if (transform.position.y < upperLimit)
        {
            transform.position += Vector3.up * Time.deltaTime;
        }
        else
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= stopTime)
            {
                direction = Direction.DOWN;
                elapsedTime = 0;
            }
        }
    }

    /// <summary>
    /// 下移動
    /// </summary>
    void Move_Down()
    {
        if (direction != Direction.DOWN) return;

        if (transform.position.y > downerLimit)
        {
            transform.position += Vector3.down * Time.deltaTime;
        }
        else
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= stopTime)
            {
                direction = Direction.UP;
                elapsedTime = 0;
            }
        }
    }
}
