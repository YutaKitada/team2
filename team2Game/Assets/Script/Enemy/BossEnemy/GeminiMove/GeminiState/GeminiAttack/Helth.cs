﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helth : MonoBehaviour
{

    [SerializeField]
    private int hp =10;


    [SerializeField]
    private GameObject hitParticl;
    Animator anime;
    [SerializeField]
    private Vector3 playerVec;
    private bool isHit;

    private float maxTime = 2;
    private float currentTime;
    
    private bool dead;
    public bool Dead
    {
        get
        {
            return dead;
        }
        private set
        {
            dead = value;
        }
    }

    [SerializeField]
    float intervalTime = 1;
    float elapsedTime;
    [SerializeField]
    private int seNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
        dead = false;
        isHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        Death();
        if(isHit)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= maxTime)
            {
                currentTime = 0;
                isHit = false;
            }
        }
        

    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.transform.GetComponent<PlayerController>().Damage(playerVec);
        }

        
        if (col.gameObject.tag == "Star"&&!isHit)
        {
            isHit = true;
            Instantiate(hitParticl, col.contacts[0].point, Quaternion.identity);
            
            if(hp <= 0)
            {
                return;
            }
            hp--;
            if(hp<=0)
            {
                dead = true;
                anime.SetTrigger("IsDead");
                SoundManager.PlaySE(seNumber);
            }
        }

    }
    
    public void Death()
    {
        if(dead)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >=intervalTime)
            {
                BossClear.SubCount(1);
                Destroy(gameObject);
            }
        }
    }
}
