using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wish_002 : Wish
{
    public override void WishStart()
    {
        UIManager.comboGageStop = true;
    }

    public override void WishUpdate()
    {

    }

    public override void WishEnd()
    {
        UIManager.comboGageStop = false;
    }
}
