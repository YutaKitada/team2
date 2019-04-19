using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject player;

    [SerializeField]
    private Vector3 cameraPosition = new Vector3(0, 4, -8);

    enum CameraMoveStop {NONE,X,Y}
    [SerializeField]
    private CameraMoveStop cameraMoveStop;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraMoveStop == CameraMoveStop.Y)
        {
            transform.position = player.transform.position + cameraPosition;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        else if(cameraMoveStop == CameraMoveStop.X)
        {
            transform.position = player.transform.position + cameraPosition;
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
    }
}
