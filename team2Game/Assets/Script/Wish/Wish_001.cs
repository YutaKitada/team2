﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wish_001 : Wish
{
    public override void WishStart()
    {
        PlayerManager.isInvincible = true;
    }

    public override void WishUpdate()
    {
        PlayerManager.isInvincible = true;
    }

    public override void WishEnd()
    {
        PlayerManager.isInvincible = false;
    }
}
