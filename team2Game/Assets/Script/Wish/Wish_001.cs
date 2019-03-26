﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wish_001 : Wish
{
    public override void WishStart()
    {
        WishManager.player.transform.localScale *= 2;
    }

    public override void WishUpdate()
    {
        
    }

    public override void WishEnd()
    {
        WishManager.player.transform.localScale /= 2;
    }
}
