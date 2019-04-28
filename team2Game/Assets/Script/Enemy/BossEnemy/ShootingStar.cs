﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShootingStar : MonoBehaviour
{
    Rigidbody rigid;
    [SerializeField]
    GameObject marker;

    Deneb deneb;
    float speed = 0;

    private void Awake()
    {
        deneb = GameObject.Find("Deneb").GetComponent<Deneb>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        if (deneb.OnRight)
        {
            speed = -2;
        }
        else
        {
            speed = 2;
        }
        SetMarker();
    }

    // Update is called once per frame
    void Update()
    {
        rigid.velocity += new Vector3(speed, 0);
    }

    void SetMarker()
    {
        if (marker == null) return;

        Vector3 direction = Vector3.zero;
        if (deneb.OnRight) direction = Vector3.left;
        else direction = Vector3.right;

        Ray ray = new Ray(transform.position, direction);
        var list = new List<RaycastHit>(Physics.RaycastAll(ray));

        //進行方向に沿って、リストにソートをする
        if (deneb.OnRight)
        {
            list.Sort((i, j) => (int)(j.point.x - i.point.x) * 100);
        }
        else
        {
            list.Sort((i, j) => (int)(i.point.x - j.point.x) * 100);
        }

        list.RemoveAll(i => i.transform.tag == "BossEnemy");
        list.RemoveAll(i => i.transform.tag == "Player");
        list.RemoveAll(i => i.transform.name.Contains("Star"));
        Instantiate(marker, list[0].point, Quaternion.identity);
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
