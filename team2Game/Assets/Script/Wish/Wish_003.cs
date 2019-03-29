using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wish_003 : Wish
{
    public override void WishStart()
    {
        UIManager.hpGageStop = true;
    }

    public override void WishUpdate()
    {
        
    }

    public override void WishEnd()
    {
        UIManager.hpGageStop = false;
    }
}
