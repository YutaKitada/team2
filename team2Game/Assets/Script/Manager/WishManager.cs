using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishManager : MonoBehaviour
{
    [SerializeField]
    private List<Wish> wishList;
    public static List<Wish> wishs;

    private string wish_one, wish_two, wish_three;

    private bool isWishMode;
    private bool isWish;

    public static GameObject player;            //Playerオブジェクト
    public static GameObject star;              //Starオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        wishs = wishList;

        wish_one = "〇";
        wish_two = "〇";
        wish_three = "〇";

        isWish = false;
        isWishMode = false;

        //Player取得
        player = GameObject.FindGameObjectWithTag("Player");
        //Star取得
        star = GameObject.FindGameObjectWithTag("Star");
    }

    // Update is called once per frame
    void Update()
    {
        WishText();
    }

    private void WishText()
    {
        if (PlayerManager.isWishMode)
        {
            if (!isWishMode)
            {
                isWishMode = true;
            }
            else
            {
                if (Input.GetButtonDown("AButton"))
                {
                    if (wish_one == "〇")
                    {
                        wish_one = "A";
                    }
                    else if (wish_two == "〇")
                    {
                        wish_two = "A";
                    }
                    else if (wish_three == "〇")
                    {
                        wish_three = "A";
                    }
                }
                else if (Input.GetButtonDown("BButton"))
                {
                    if (wish_one == "〇")
                    {
                        wish_one = "B";
                    }
                    else if (wish_two == "〇")
                    {
                        wish_two = "B";
                    }
                    else if (wish_three == "〇")
                    {
                        wish_three = "B";
                    }
                }
                else if (Input.GetButtonDown("XButton"))
                {
                    if (wish_one == "〇")
                    {
                        wish_one = "X";
                    }
                    else if (wish_two == "〇")
                    {
                        wish_two = "X";
                    }
                    else if (wish_three == "〇")
                    {
                        wish_three = "X";
                    }
                }
                else if (Input.GetButtonDown("YButton"))
                {
                    if (wish_one == "〇")
                    {
                        wish_one = "Y";
                    }
                    else if (wish_two == "〇")
                    {
                        wish_two = "Y";
                    }
                    else if (wish_three == "〇")
                    {
                        wish_three = "Y";

                    }
                }

                UIManager.wishText = wish_one + wish_two + wish_three;

                if (wish_one != "〇" && wish_two != "〇" && wish_three != "〇" && !isWish)
                {
                    Wish(UIManager.wishText);
                    isWish = true;
                }
            }
        }
        else
        {
            isWish = false;
            wish_one = "〇";
            wish_two = "〇";
            wish_three = "〇";
            isWishMode = false;
        }
        
    }


    public static void Wish(string command)
    {
        foreach(var n in wishs)
        {
            if(n.wishCommand == command)
            {
                n.startWish = true;
            }
        }
    }
}
