using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : Projectile
{
    public Transform target;
    public float speed;
    public float homingSensitivity = 0.9f;  //How sensitive the missle is to changing direction towards target
    public float highscoreReached1, highscoreReached2, highscoreReached3; //values to be reached to unlock better shot delay
    public float delayBetweenShots0, delayBetweenShots1, delayBetweenShots2, delayBetweenShots3; //better shot delays
    private GameObject _buddyShip;
    private Weapon _weapon;
    private BoundsCheck _bndChecks;


    void Start()
    {
        _buddyShip = GameObject.FindGameObjectWithTag("BuddyShip");
        _weapon = _buddyShip.GetComponentInChildren<Weapon>();
        _bndChecks = GetComponent<BoundsCheck>();
        highscoreBasedFireRate();
    }

    void Update()
    {
        //If missileProjectile goes off screen, destroy it
        if (_bndChecks.offUp || _bndChecks.offDown || _bndChecks.offLeft || _bndChecks.offRight)
        {
            Destroy(gameObject);
        }

        //This code finds an enemy and tracks it
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if(enemy != null)
        target = enemy.transform; //sets target to be the enemy's transform
        if (target != null)
        {
            Vector3 relativePos = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos); //Sets missile to gradually point at target
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, homingSensitivity);
        }

        transform.Translate(0, 0, speed * Time.deltaTime, Space.Self); //moves missile projectile
    }

    //This function determines which delayBetweenShots the missiles will fire at based on the acheived highscore
    //A larger highscore means a smaller delayBetweenShots
    void highscoreBasedFireRate()
    {
        _weapon.def.delayBetweenShots = delayBetweenShots0;
        if (ScoreManager._HIGH_SCORE >= highscoreReached1)
        {
            _weapon.def.delayBetweenShots = delayBetweenShots1;
            if (ScoreManager._HIGH_SCORE >= highscoreReached2)
            {
                _weapon.def.delayBetweenShots = delayBetweenShots2;
                if (ScoreManager._HIGH_SCORE >= highscoreReached3)
                {
                    _weapon.def.delayBetweenShots = delayBetweenShots3;
                }
            }

        }
    }
}

