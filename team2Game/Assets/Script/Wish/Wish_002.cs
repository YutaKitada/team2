using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wish_002 : Wish
{
    public override void WishStart()
    {
        WishManager.star.transform.localScale *= 2;
    }

    public override void WishUpdate()
    {

    }

    public override void WishEnd()
    {
        WishManager.star.transform.localScale /= 2;
    }
}
