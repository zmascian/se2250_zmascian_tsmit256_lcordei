using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    //Change in inspector
    public float radius;
    public bool keepOnScreen;
    public bool isOnScreen;
    public float camHeight;
    public float camWidth;
    public bool offRight, offLeft, offUp, offDown;

    void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        isOnScreen = true;
        offRight = offLeft = offUp = offDown = false;

        //Checking if object is off on the right of the screen
        if (pos.x > camWidth - radius)
        {
            pos.x = camWidth - radius; //Prevent object from moving in the same direction
            isOnScreen = false;
            offRight = true;
        }

        //Checking if object is off on the left of the screen
        if (pos.x < -camWidth + radius)
        {
            pos.x = -camWidth + radius; //Prevent object from moving in the same direction
            isOnScreen = false;
            offLeft = true;
        }

        //Checking if object is off the upper part of the screen
        if (pos.y > camHeight - radius)
        {
            pos.y = camHeight - radius; //Prevent object from moving in the same direction
            isOnScreen = false;
            offUp = true;
        }

        //Checking if object is off the lower part of the screen
        if (pos.y < -camHeight + radius)
        {
            pos.y = -camHeight + radius; //Prevent object from moving in the same direction
            isOnScreen = false;
            offDown = true;
        }

        isOnScreen = !(offRight || offLeft || offUp || offDown);
        if (keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            isOnScreen = true;
            offRight = offLeft = offUp = offDown = false;
        }
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
