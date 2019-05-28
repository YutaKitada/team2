using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// デネブが生成する上からの攻撃用の星
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class FallingStar : MonoBehaviour
{
    Rigidbody rigid;

    [SerializeField, Header("表示させるマーカー")]
    GameObject marker;
    GameObject setObj;//生成したマーカーを格納する変数

    Deneb deneb;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        deneb = GameObject.FindGameObjectWithTag("BossEnemy").GetComponent<Deneb>();
        SetMarker();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.isWishMode || WishManager.wishProductionFlag) return;

        rigid.velocity += new Vector3(0, -9.8f * Time.deltaTime);
        transform.Rotate(new Vector3(0, 5, 0));

        if (deneb.Hp <= 0)
        {
            RemoveObjects();
        }
    }

    /// <summary>
    /// 落下地点にマーカーを生成
    /// </summary>
    void SetMarker()
    {
        if (marker == null) return;//prefab設定がなければreturn

        //ステージ上に生成させる
        Ray ray = new Ray(transform.position, Vector3.down);
        var list = new List<RaycastHit>(Physics.RaycastAll(ray));

        //上から当たった順にリストをソート
        list.Sort((i, j) => (int)(j.point.y - i.point.y) * 100);
        //ボス、プレイヤー、星以外のオブジェクトをリストから除外
        list.RemoveAll(i => i.transform.tag == "BossEnemy");
        list.RemoveAll(i => i.transform.tag == "Player");
        list.RemoveAll(i => i.transform.tag == "Star");

        //rayが通ったリストの一番最初のオブジェクトにrayが当たった位置に表示
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

        SoundManager.PlaySE(21);
        RemoveObjects();
    }
}
