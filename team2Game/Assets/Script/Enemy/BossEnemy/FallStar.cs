using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallStar : MonoBehaviour
{
    Rigidbody rigid;

    [SerializeField]
    GameObject marker;

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
    }

    void SetMarker()
    {
        if (marker == null) return;

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            Instantiate(marker, hit.point, Quaternion.identity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            PlayerManager.PlayerDamage(10);
        }

        Destroy(gameObject);
    }
}
