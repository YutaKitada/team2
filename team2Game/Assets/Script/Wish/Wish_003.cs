using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wish_003 : Wish
{
    public override void WishStart()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var n in enemys)
        {
            Destroy(n.gameObject);
        }
    }

    public override void WishUpdate()
    {
        
    }

    public override void WishEnd()
    {

    }
}
