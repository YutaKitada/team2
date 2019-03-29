using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishManager : MonoBehaviour
{
    [SerializeField]
    private List<Wish> wishList;
    public static List<Wish> wishs;

    private string wish_one, wish_two, wish_three;
    private string wishCommand;

    private bool isWishMode;
    private bool isWish;

    private bool stickLock;

    public static GameObject player;            //Playerオブジェクト
    private MeshRenderer playerRender;
    public static GameObject star;              //Starオブジェクト

    public static bool isWishNow;               //現在願い事効果中か否か

    // Start is called before the first frame update
    void Start()
    {
        wishs = wishList;

        wish_one = "〇";
        wish_two = "〇";
        wish_three = "〇";

        wishCommand = "";

        isWish = false;
        isWishMode = false;

        stickLock = false;

        //Player取得
        player = GameObject.FindGameObjectWithTag("Player");
        playerRender = player.GetComponent<MeshRenderer>();
        //Star取得
        star = GameObject.FindGameObjectWithTag("Star");

        isWishNow = false;
    }

    // Update is called once per frame
    void Update()
    {
        WishText();

        if (PlayerManager.isWishStay)
        {
            playerRender.material.color = Color.blue;
        }
        else if (!PlayerManager.isWishStay)
        {
            playerRender.material.color = Color.red;
        }
    }

    private void WishText()
    {
        if (PlayerManager.isWishMode)
        {
            if (!isWishMode)
            {
                isWishMode = true;
                GameManager.isGameStop = true;
            }
            else
            {
                if (!isWish)
                {
                    if (Input.GetButtonDown("AButton"))
                    {
                        //if (wish_one == "〇")
                        //{
                        //    wish_one = "A";
                        //}
                        //else if (wish_two == "〇")
                        //{
                        //    wish_two = "A";
                        //}
                        //else if (wish_three == "〇")
                        //{
                        //    wish_three = "A";
                        //}
                        wishCommand += "A";
                    }
                    else if (Input.GetButtonDown("BButton"))
                    {
                        //if (wish_one == "〇")
                        //{
                        //    wish_one = "B";
                        //}
                        //else if (wish_two == "〇")
                        //{
                        //    wish_two = "B";
                        //}
                        //else if (wish_three == "〇")
                        //{
                        //    wish_three = "B";
                        //}
                        wishCommand += "B";
                    }
                    else if (Input.GetButtonDown("XButton"))
                    {
                        //if (wish_one == "〇")
                        //{
                        //    wish_one = "X";
                        //}
                        //else if (wish_two == "〇")
                        //{
                        //    wish_two = "X";
                        //}
                        //else if (wish_three == "〇")
                        //{
                        //    wish_three = "X";
                        //}
                        wishCommand += "X";
                    }
                    else if (Input.GetButtonDown("YButton"))
                    {
                        //if (wish_one == "〇")
                        //{
                        //    wish_one = "Y";
                        //}
                        //else if (wish_two == "〇")
                        //{
                        //    wish_two = "Y";
                        //}
                        //else if (wish_three == "〇")
                        //{
                        //    wish_three = "Y";

                        //}
                        wishCommand += "Y";
                    }
                    else if (Input.GetAxisRaw("Horizontal") >= 1 &&!stickLock)
                    {
                        wishCommand += "→";
                        stickLock = true;
                    }
                    else if (Input.GetAxisRaw("Horizontal") <= -1 && !stickLock)
                    {
                        wishCommand += "←";
                        stickLock = true;
                    }
                    else if (Input.GetAxisRaw("Vertical") <= -1 && !stickLock)
                    {
                        wishCommand += "↑";
                        stickLock = true;
                    }
                    else if (Input.GetAxisRaw("Vertical") >= 1 && !stickLock)
                    {
                        wishCommand += "↓";
                        stickLock = true;
                    }
                    else if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
                    {
                        stickLock = false;
                    }
                    //UIManager.wishText = wish_one + wish_two + wish_three;
                    UIManager.wishText = wishCommand;

                    Wish(UIManager.wishText);

                    //if (/*wish_one != "〇" && wish_two != "〇" && wish_three != "〇" && */!isWish)
                    //{

                    //    isWish = true;
                    //}
                }

            }
        }
        else
        {
            isWish = false;
            wish_one = "〇";
            wish_two = "〇";
            wish_three = "〇";
            wishCommand = "";
            UIManager.wishText = wishCommand;
            isWishMode = false;
            stickLock = false;
            GameManager.isGameStop = false;
        }
        
    }


    private void Wish(string command)
    {
        foreach(var n in wishs)
        {
            if(n.wishCommand == command)
            {
                n.startWish = true;
                isWish = true;
                isWishNow = true;
            }
        }
    }
}
