using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    [SerializeField, Header("振動時間")]
    float shakeTime = 0.5f;

    float lifeTime;
    Vector3 savePposition;
    float lowRangeX;
    float maxRangeX;
    float lowRangeY;
    float maxRangeY;

    // Start is called before the first frame update
    void Start()
    {
        if(shakeTime <= 0)
        {
            shakeTime = 0.7f;
        }
        lifeTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTime < 0)
        {
            transform.position = savePposition;
            lifeTime = 0;
        }

        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            float x_value = Random.Range(lowRangeX, maxRangeX);
            float y_value = Random.Range(lowRangeY, maxRangeY);
            transform.position = new Vector3(x_value, y_value, transform.position.z);
        }
    }

    /// <summary>
    /// カメラ振動
    /// </summary>
    public void Shake()
    {
        if (PlayerManager.isWishMode || WishManager.wishProductionFlag) return;

        savePposition = transform.position;
        lowRangeY = savePposition.y - 1f;
        maxRangeY = savePposition.y + 1f;
        lowRangeX = savePposition.x - 1f;
        maxRangeX = savePposition.x + 1f;
        lifeTime = shakeTime;
    }
}
