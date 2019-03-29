using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wish_004 : Wish
{
    public override void WishStart()
    {
        
    }

    public override void WishUpdate()
    {
        if(!PlayerManager.haveStar && WishManager.star.GetComponent<StarMovement>().returnPlayer)
        {
            PlayerManager.haveStar = true;
            WishManager.star.GetComponent<StarMovement>().returnPlayer = false;
        }
    }

    public override void WishEnd()
    {

    }
}