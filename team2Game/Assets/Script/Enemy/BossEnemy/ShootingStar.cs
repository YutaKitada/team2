using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// デネブが生成する横からの攻撃用の星
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ShootingStar : MonoBehaviour
{
    Rigidbody rigid;
    [SerializeField, Header("表示させるマーカー")]
    GameObject marker;
    GameObject setObj;//生成したマーカーを格納する変数

    Deneb deneb;
    [SerializeField, Header("横の移動速度")]
    float speedOrigin = 2;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        deneb = GameObject.FindGameObjectWithTag("BossEnemy").GetComponent<Deneb>();
        if (deneb.OnRight) speed = -speedOrigin;
        else speed = speedOrigin;
        SetMarker();
    }

    // Update is called once per frame
    void Update()
    {
        rigid.velocity += new Vector3(speed / 2f, 0);
        transform.Rotate(new Vector3(0, 5, 0));

        if (deneb.Hp <= 0)
        {
            RemoveObjects();
        }
    }

    void SetMarker()
    {
        if (marker == null) return;//prefabが設定されていなければreturn

        Vector3 direction = Vector3.zero;
        //デネブの位置に応じて方向を決める
        if (deneb.OnRight) direction = Vector3.left;
        else direction = Vector3.right;

        Ray ray = new Ray(transform.position, direction);
        var list = new List<RaycastHit>(Physics.RaycastAll(ray));

        //方向に沿って、リストのソートをする
        if (deneb.OnRight)
        {
            list.Sort((i, j) => (int)(j.point.x - i.point.x) * 100);
        }
        else
        {
            list.Sort((i, j) => (int)(i.point.x - j.point.x) * 100);
        }
        //ボス、プレイヤー、星以外のオブジェクトをリストから除外
        list.RemoveAll(i => i.transform.tag == "BossEnemy");
        list.RemoveAll(i => i.transform.tag == "Player");
        list.RemoveAll(i => i.transform.tag == "Star");

        //ソートしたリストから一番最初にあるオブジェクトにrayが当たった位置に表示
        setObj = Instantiate(marker, list[0].point, Quaternion.identity);
    }

    /// <summary>
    /// 自身と生成したマーカーを削除
    /// </summary>
    void RemoveObjects()
    {
        Destroy(setObj);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            PlayerManager.PlayerDamage(10);
        }

        RemoveObjects();
    }
}
