﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    private Vector3 p0, p1;
    private float _timeStart;
    private float _direction;
    private float _duration = 4;
    public Weapon _weapon;
    public float gameRestartDelay = 2f;



    void Start()
    {
        _weapon = gameObject.GetComponentInChildren<Weapon>();
        p0 = p1 = pos;
        InitMovement();

    }
    void InitMovement()
    {
        p0 = p1;
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = Random.Range(-widMinRad , widMinRad / 2.0f);
        p1.y = Random.Range(-hgtMinRad / 3.0f, hgtMinRad / 3.0f);

        _timeStart = Time.time;
    }

    public override void Move()
    {
        
        _weapon.Fire(); //Execute the fire function of the weapon component each time move is called
        //Actual fire rate will depend on settings in weapon class
        

        //Same style as seen in Enemy_2 for its swoop but this is much more tame
        float u = (Time.time - _timeStart) / _duration;

        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);
        pos = (1 - u) * p0 + u * p1;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;
        switch (otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                //If offscreen, don't damage
                if (!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }
                //Hurt this enemy and get damage amount from WEAP_DICT
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if (health <= 0)
                {
                    FindObjectOfType<SoundManager>().Play("destroyAudio");
                    Destroy(this.gameObject); //Destory the enemy when it is out of health
                    AddToScore(); //Add to score when the enemy is dead
                }
                Destroy(otherGO);
                break;
        }
    }


    //7 points are gained for each Enemy_3 gameobject that is destroyed
    public override void AddToScore()
    {
        ScoreManager.ADD_POINTS(7);
    }
}
