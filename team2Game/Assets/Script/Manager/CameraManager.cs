using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject player;

    [SerializeField]
    private Vector3 cameraPosition = new Vector3(0, 4, -8);

    enum CameraMoveStop {NONE,X,Y,BOSS}
    [SerializeField]
    private CameraMoveStop cameraMoveStop;

    [SerializeField]
    private float moveSpeedX = 7f;
    [SerializeField]
    private float moveSpeedY = 3f;

    enum CameraMoveDirectionX {LEFT,RIGHT,NONE};
    private CameraMoveDirectionX cameraMoveDirectionX;

    enum CameraMoveDirectionY { UP, DOWN, NONE };
    private CameraMoveDirectionY cameraMoveDirectionY;

    private GameObject goalObject;

    private bool positionChange;

    private Vector3 firstPosition;

    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        if(GameObject.Find("Goal") != null)
        {
            goalObject = GameObject.Find("Goal");
        }
        cameraMoveDirectionX = CameraMoveDirectionX.NONE;
        cameraMoveDirectionY = CameraMoveDirectionY.NONE;
        transform.position = player.transform.position + cameraPosition;
        switch (cameraMoveStop)
        {
            case CameraMoveStop.Y:
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                break;
            case CameraMoveStop.X:
                transform.position = new Vector3(0, transform.position.y, transform.position.z);
                break;
        }
        positionChange = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraMoveStop == CameraMoveStop.Y)
        {
            
            
            
            float distance = player.transform.position.x - transform.position.x;

            if(distance >= 0.5f)
            {
                cameraMoveDirectionX = CameraMoveDirectionX.RIGHT;
            }
            else if (distance <= -0.5f)
            {
                cameraMoveDirectionX = CameraMoveDirectionX.LEFT;
                
            }
            else if(distance >= -0.2f && distance <= 0.2f)
            {
                cameraMoveDirectionX = CameraMoveDirectionX.NONE;
            }
            switch (cameraMoveDirectionX)
            {
                case CameraMoveDirectionX.RIGHT:
                    transform.position += new Vector3(1, 0) * moveSpeedX *(distance / 5)* Time.deltaTime;
                    break;
                case CameraMoveDirectionX.LEFT:
                    transform.position -= new Vector3(1, 0) * moveSpeedX * (distance / -5) * Time.deltaTime;
                    break;
            }
        }
        else if(cameraMoveStop == CameraMoveStop.X)
        {
            //transform.position = player.transform.position + cameraPosition;
            

            float distance = player.transform.position.y - transform.position.y;

            if (distance >= 0.3f)
            {
                cameraMoveDirectionY = CameraMoveDirectionY.UP;
            }
            else if (distance <= -0.3f)
            {
                cameraMoveDirectionY = CameraMoveDirectionY.DOWN;

            }
            else if (distance >= -0.2f && distance <= 0.2f)
            {
                cameraMoveDirectionY = CameraMoveDirectionY.NONE;
            }
            switch (cameraMoveDirectionY)
            {
                case CameraMoveDirectionY.UP:
                    transform.position += new Vector3(0, 1) * moveSpeedY * (distance / 3) * Time.deltaTime;
                    break;
                case CameraMoveDirectionY.DOWN:
                    transform.position -= new Vector3(0, 1) * moveSpeedY * (distance / -3) * Time.deltaTime;
                    break;
            }
        }
        else if(cameraMoveStop == CameraMoveStop.NONE)
        {
            float distanceX = player.transform.position.x - transform.position.x;

            if (distanceX >= 0.5f)
            {
                cameraMoveDirectionX = CameraMoveDirectionX.RIGHT;
            }
            else if (distanceX <= -0.5f)
            {
                cameraMoveDirectionX = CameraMoveDirectionX.LEFT;

            }
            else if (distanceX >= -0.2f && distanceX <= 0.2f)
            {
                cameraMoveDirectionX = CameraMoveDirectionX.NONE;
            }
            switch (cameraMoveDirectionX)
            {
                case CameraMoveDirectionX.RIGHT:
                    transform.position += new Vector3(1, 0) * moveSpeedX * (distanceX / 5) * Time.deltaTime;
                    break;
                case CameraMoveDirectionX.LEFT:
                    transform.position -= new Vector3(1, 0) * moveSpeedX * (distanceX / -5) * Time.deltaTime;
                    break;
            }


            float distanceY = player.transform.position.y - transform.position.y;

            if (distanceY >= 0.3f)
            {
                cameraMoveDirectionY = CameraMoveDirectionY.UP;
            }
            else if (distanceY <= -0.3f)
            {
                cameraMoveDirectionY = CameraMoveDirectionY.DOWN;

            }
            else if (distanceY >= -0.2f && distanceY <= 0.2f)
            {
                cameraMoveDirectionY = CameraMoveDirectionY.NONE;
            }
            switch (cameraMoveDirectionY)
            {
                case CameraMoveDirectionY.UP:
                    transform.position += new Vector3(0, 1) * moveSpeedY * (distanceY / 3) * Time.deltaTime;
                    break;
                case CameraMoveDirectionY.DOWN:
                    transform.position -= new Vector3(0, 1) * moveSpeedY * (distanceY / -3) * Time.deltaTime;
                    break;
            }
        }
        else if(cameraMoveStop == CameraMoveStop.BOSS)
        {
            transform.position = cameraPosition;
        }
       

        if (PlayerManager.isWishMode || WishManager.wishProductionFlag)
        {
            transform.position = new Vector3(WishManager.player.transform.position.x, WishManager.player.transform.position.y + 1, -14);
            GetComponent<Camera>().fieldOfView = 60;
            positionChange = true;
        }
        else
        {
            if(goalObject != null)
            {
                float goalDistance = Vector3.Distance(player.transform.position, goalObject.transform.position);

                Vector3 cameraDistance = Vector3.Lerp(goalObject.transform.position + cameraPosition - new Vector3(0,0,cameraPosition.z/2), player.transform.position + cameraPosition, goalDistance / 9);
                switch (cameraMoveStop)
                {
                    case CameraMoveStop.Y:
                        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                        break;
                    case CameraMoveStop.X:
                        transform.position = new Vector3(0, transform.position.y, transform.position.z);
                        break;
                }
                if (goalDistance < 9)
                {
                    transform.position = new Vector3(WishManager.player.transform.position.x, WishManager.player.transform.position.y + 1, cameraDistance.z);
                }
                

                GetComponent<Camera>().fieldOfView = 90;
                if (positionChange)
                {
                    positionChange = false;
                    cameraMoveDirectionX = CameraMoveDirectionX.NONE;
                    cameraMoveDirectionY = CameraMoveDirectionY.NONE;
                    transform.position = player.transform.position + cameraPosition;
                    switch (cameraMoveStop)
                    {
                        case CameraMoveStop.Y:
                            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                            break;
                        case CameraMoveStop.X:
                            transform.position = new Vector3(0, transform.position.y, transform.position.z);
                            break;
                    }
                    GetComponent<Camera>().fieldOfView = 90;
                }

            }
            else
            {
                if (positionChange)
                {
                    positionChange = false;
                    cameraMoveDirectionX = CameraMoveDirectionX.NONE;
                    cameraMoveDirectionY = CameraMoveDirectionY.NONE;
                    transform.position = player.transform.position + cameraPosition;
                    switch (cameraMoveStop)
                    {
                        case CameraMoveStop.Y:
                            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                            break;
                        case CameraMoveStop.X:
                            transform.position = new Vector3(0, transform.position.y, transform.position.z);
                            break;
                    }
                    GetComponent<Camera>().fieldOfView = 90;
                }
               // transform.position = new Vector3(WishManager.player.transform.position.x, WishManager.player.transform.position.y + 1, -9);
                
            }
        }
    }
}
