﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed, fireRate, health, score;
    protected BoundsCheck bndCheck;


    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }


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
  

    // Update is called once per frame.
    void Update()
    {
        Move();   

        if( bndCheck != null && (bndCheck.offDown || bndCheck.offLeft|| bndCheck.offRight))
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

    void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;
        //If otherGO is projectileHero, destroy it and destroy this enemy instance
        if(otherGO.tag == "ProjectileHero")
        {
            Destroy(otherGO);
            Destroy(gameObject);
        }
        //If otherGO wasn't projectileHero, print the name of what was hit for debugging purposes
        else
        {
            print("Enemy hit by non-ProjectileHero: " + otherGO.name);
        }
    }
}
