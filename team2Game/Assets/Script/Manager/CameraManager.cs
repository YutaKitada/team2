﻿using System.Collections;
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

    [SerializeField]
    private float moveSpeedX = 7f;
    [SerializeField]
    private float moveSpeedY = 3f;

    enum CameraMoveDirectionX {LEFT,RIGHT,NONE};
    private CameraMoveDirectionX cameraMoveDirectionX;

    enum CameraMoveDirectionY { UP, DOWN, NONE };
    private CameraMoveDirectionY cameraMoveDirectionY;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cameraMoveDirectionX = CameraMoveDirectionX.NONE;
        cameraMoveDirectionY = CameraMoveDirectionY.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraMoveStop == CameraMoveStop.Y)
        {
            //transform.position = player.transform.position + cameraPosition;
            //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            
            float distance = player.transform.position.x - transform.position.x;

            if(distance >= 5)
            {
                cameraMoveDirectionX = CameraMoveDirectionX.RIGHT;
            }
            else if (distance <= -5)
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
            //transform.position = new Vector3(0, transform.position.y, transform.position.z);

            float distance = player.transform.position.y - transform.position.y;

            if (distance >= 3)
            {
                cameraMoveDirectionY = CameraMoveDirectionY.UP;
            }
            else if (distance <= -3)
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

            if (distanceX >= 5)
            {
                cameraMoveDirectionX = CameraMoveDirectionX.RIGHT;
            }
            else if (distanceX <= -5)
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

            if (distanceY >= 3)
            {
                cameraMoveDirectionY = CameraMoveDirectionY.UP;
            }
            else if (distanceY <= -3)
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
    }
}
