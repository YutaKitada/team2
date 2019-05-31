using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NameChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        transform.name = SceneManager.GetActiveScene().name;
    }
}
