using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishManager : MonoBehaviour
{
    [SerializeField]
    private List<string> wishCommandList;
    public static List<string> wishCommand;

    // Start is called before the first frame update
    void Start()
    {
        wishCommand = wishCommandList;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Wish(string command)
    {
        if (command == "X　X　X")
        {
            GameManager.star.transform.localScale *= 2;
        }
        if (command == "Y　Y　Y")
        {
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(var n in enemys)
            {
                if(UIManager.gageFillAmount <= 100)
                {
                    UIManager.gageFillAmount += 10f;
                }
                GameManager.combo++;
                UIManager.isCombo = true;
                UIManager.gageStopTimer = 0;
                Destroy(n);
            }
        }
        if (command == "B　B　B")
        {
            GameManager.player.transform.localScale *= 2;
        }
        if (command == "A　A　A")
        {
            GameManager.star.transform.localScale /= 2;
            GameManager.player.transform.localScale /= 2;
        }
    }
}
