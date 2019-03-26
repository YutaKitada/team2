using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wish : MonoBehaviour
{
    public enum WishCurrent {START,UPDATE,END};
    [HideInInspector]
    private WishCurrent wishCurrent;

    public string wishCommand;

    [SerializeField]
    private float wishTime = 5;
    private float wishTimer;

    private bool isWishEnd;

    [HideInInspector]
    public bool startWish;

    // Start is called before the first frame update
    void Start()
    {
        wishCurrent = WishCurrent.START;
        wishTimer = 0;

        isWishEnd = false;
        startWish = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startWish)
        {
            switch (wishCurrent)
            {
                case WishCurrent.START:
                    WishStart();
                    wishCurrent = WishCurrent.UPDATE;
                    break;
                case WishCurrent.UPDATE:
                    WishUpdate();
                    wishTimer += Time.deltaTime;
                    if (wishTimer >= wishTime)
                    {
                        wishCurrent = WishCurrent.END;
                    }
                    break;
                case WishCurrent.END:
                    startWish = false;
                    WishEnd();
                    wishTimer = 0;
                    wishCurrent = WishCurrent.START;
                    break;
            }
        }
    }

    public virtual void WishStart()
    {

    }

    public virtual void WishUpdate()
    {

    }

    public virtual void WishEnd()
    {

    }
}
