using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallingStar : MonoBehaviour
{
    Rigidbody rigid;

    [SerializeField]
    GameObject marker;
    GameObject setObj;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        SetMarker();
    }

    // Update is called once per frame
    void Update()
    {
        rigid.velocity += new Vector3(0, -9.8f * Time.deltaTime, 0);
        transform.Rotate(new Vector3(0, 5, 0));
    }

    void SetMarker()
    {
        if (marker == null) return;

        //ステージ上に生成させる
        Ray ray = new Ray(transform.position, Vector3.down);
        var list = new List<RaycastHit>(Physics.RaycastAll(ray));
        list.Sort((i, j) => (int)(j.point.y - i.point.y) * 100);
        list.RemoveAll(i => i.transform.tag == "BossEnemy");
        list.RemoveAll(i => i.transform.tag == "Player");
        list.RemoveAll(i => i.transform.name.Contains("Star"));
        setObj = Instantiate(marker, list[0].point, Quaternion.identity);
    }

    /// <summary>
    /// 星と生成したマーカーを削除
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
