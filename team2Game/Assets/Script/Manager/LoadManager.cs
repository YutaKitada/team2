using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage;
    public static bool isFade;

    // Start is called before the first frame update
    void Start()
    {
        isFade = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void GoNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
