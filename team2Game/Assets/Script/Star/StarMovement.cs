using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMovement : MonoBehaviour
{

    private Rigidbody rigid;

    [HideInInspector]
    public bool returnPlayer;

    [SerializeField]
    private float returnPower = 10;
    private float x;

    private MeshRenderer render;

    private bool isTimerStart;

    private bool inWater;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        returnPlayer = false;
        
        x = 0;

        render = GetComponent<MeshRenderer>();

        isTimerStart = true;
        inWater = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (returnPlayer)
        {
            Vector3 returnVector = GameManager.player.transform.position - transform.position;

            returnVector = returnVector.normalized;

            //rigid.AddForce(returnVector * returnPower);
            //rigid.velocity = returnVector * returnPower;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!returnPlayer)
        {
            returnPlayer = true;
            PlayerManager.isWishMode = false;
            gameObject.layer = 11;

            if (collision.transform.tag == "Enemy")
            {
                if (UIManager.gageFillAmount <= 100)
                {
                    if (UIManager.isCombo)
                    {
                        UIManager.gageFillAmount += 10f * (1 + (float)GameManager.combo * 0.2f) ;
                    }
                    else
                    {
                        UIManager.gageFillAmount += 10f;
                    }
                }
                else
                {
                    UIManager.gageFillAmount += 0.5f;
                }
                if (!UIManager.isCombo)
                {
                    UIManager.isCombo = true;
                }
                GameManager.combo++;
            }
            else if (collision.transform.tag != "Player")
            {
                UIManager.gageFillAmount -= 5f;

                if (UIManager.isCombo)
                {
                    GameManager.combo = 0;
                    UIManager.isCombo = false;
                }
            }

            if (!inWater)
            {
                rigid.velocity = Vector3.zero;

                Vector3 vector = GameManager.player.transform.position - transform.position;

                if (vector.x > 0)
                {
                    x = 5 + GameManager.combo;
                }
                else if (vector.x < 0)
                {
                    x = -5 - GameManager.combo;
                }

                rigid.AddForce(new Vector3(x, 8), ForceMode.Impulse);
            }

            

            if (!isTimerStart)
            {
                UIManager.gageStopTimer = 0;
                isTimerStart = true;
            }
        }

        if (collision.transform.tag == "Player")
        {
            returnPlayer = false;
            isTimerStart = false;
            inWater = false;
            rigid.drag = 0f;
            //rigid.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Water")
        {
            inWater = true;
            rigid.drag = 1f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
        {
            inWater = false;
            rigid.drag = 0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Water")
        {
            rigid.AddForce(new Vector3(0, 15));
        }
    }
}
