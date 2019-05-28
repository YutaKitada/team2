using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helth : MonoBehaviour
{

    [SerializeField]
    private int hp =10;


    [SerializeField]
    private GameObject hitParticl;
    Animator anime;

    public bool dead;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        Death();
    }
    private void OnCollisionEnter(Collision col)
    {

        
        if (col.gameObject.tag == "Star")
        {
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
                Destroy(gameObject);
            }
        }
    }
}
