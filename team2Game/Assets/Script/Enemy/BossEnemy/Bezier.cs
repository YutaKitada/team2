using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ベジェ曲線を描くためのスクリプト
/// </summary>
public class Bezier : MonoBehaviour
{
    /// <summary>
    /// ベジェ曲線を描く
    /// </summary>
    /// <param name="p0">開始地点</param>
    /// <param name="p1">制御点1</param>
    /// <param name="p2">制御点2</param>
    /// <param name="p3">終点</param>
    /// <param name="t">時間（0～1の間）</param>
    /// <returns></returns>
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        var oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * oneMinusT * p0 +
            3f * oneMinusT * oneMinusT * t * p1 +
            3f * oneMinusT * t * t * p2 +
            t * t * t * p3;
    }
}
