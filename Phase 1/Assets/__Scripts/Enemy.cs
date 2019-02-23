﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed, fireRate, health, score;
    private BoundsCheck bndCheck;

    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }
  

    // Update is called once per frame
    void Update()
    {
        Move();   

        if( bndCheck != null && !bndCheck.isOnScreen)
        {
            if (pos.y < bndCheck.camHeight - bndCheck.radius)
            {
                Destroy(gameObject);
            }
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
}
