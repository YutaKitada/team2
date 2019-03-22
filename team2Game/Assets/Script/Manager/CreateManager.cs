using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameManager;
    [SerializeField]
    private GameObject soundManager;
    [SerializeField]
    private GameObject playerManager;
    [SerializeField]
    private GameObject loadManager;
    [SerializeField]
    private GameObject uiManager;
    [SerializeField]
    private GameObject wishManager;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("GameManager(Clone)") == null)
        {
            Instantiate(gameManager);
        }
        if (GameObject.Find("SoundManager(Clone)") == null)
        {
            GameObject sound = Instantiate(soundManager);
            sound.GetComponent<SoundManager>().BGM_SE_Load();
        }
        if(GameObject.Find("PlayerManager(Clone)") == null)
        {
            Instantiate(playerManager);
        }
        if(GameObject.Find("LoadManager(Clone)") == null)
        {
            Instantiate(loadManager);
        }
        if (GameObject.Find("UIManager(Clone)") == null)
        {
            Instantiate(uiManager);
        }
        if (GameObject.Find("WishManager(Clone)") == null)
        {
            Instantiate(wishManager);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}