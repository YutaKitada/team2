using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarEffect : MonoBehaviour
{
    private GameObject star;
    private StarMovement starMovement;
    [SerializeField]
    private GameObject effect;
    [SerializeField]
    private bool wishStar;

    // Start is called before the first frame update
    void Start()
    {
        star = GameObject.FindGameObjectWithTag("Star");
        starMovement = star.GetComponent<StarMovement>();
        effect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!wishStar)
        {
            transform.position = star.transform.position;

            if (!starMovement.returnPlayer && !PlayerManager.haveStar)
            {
                if (!effect.activeSelf)
                {
                    effect.SetActive(true);
                    if (PlayerThrow.dank)
                    {
                        effect.transform.rotation = Quaternion.LookRotation(new Vector3(0, -1, 1));
                    }
                    else if (PlayerManager.playerDirection == PlayerManager.PlayerDirection.LEFT)
                    {
                        //effect.transform.rotation = Quaternion.LookRotation(new Vector3(PlayerManager.throwDirection.x, -PlayerManager.throwDirection.y));
                        effect.transform.rotation = Quaternion.LookRotation(new Vector3(PlayerManager.effectDirection.x, -PlayerManager.effectDirection.y));
                        //effect.transform.rotation = Quaternion.LookRotation(new Vector3(1, -1));
                        //effect.transform.LookAt(PlayerManager.throwDirection);
                    }
                    else if (PlayerManager.playerDirection == PlayerManager.PlayerDirection.RIGHT)
                    {
                        //effect.transform.rotation = Quaternion.LookRotation(new Vector3(PlayerManager.throwDirection.x, -PlayerManager.throwDirection.y));
                        effect.transform.rotation = Quaternion.LookRotation(new Vector3(PlayerManager.effectDirection.x, -PlayerManager.effectDirection.y));
                        //effect.transform.rotation = Quaternion.LookRotation(new Vector3(0.5f, -1));
                        //effect.transform.LookAt(PlayerManager.throwDirection);
                    }
                }

            }
            else
            {
                effect.SetActive(false);
            }
        }
        else
        {
            effect.SetActive(true);
        }
    }
}
