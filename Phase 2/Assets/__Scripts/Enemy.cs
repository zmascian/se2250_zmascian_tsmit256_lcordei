using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed, fireRate, health, score;
    protected BoundsCheck bndCheck;
    public float powerUpDropChance = 1f; //this is the chance for the enemy to drop a power-up
    private bool _notifiedOfDestruction = false;
   

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
                if(health <= 0)
                {
                    //Tell the Main singleton that this ship was destroyed
                    if (!_notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                    }
                    _notifiedOfDestruction = true;
                    Destroy(this.gameObject);

                    AddToScore();
                }
                Destroy(otherGO);
                break;

            default:
                print("Enemy hit by non-projectileHero: " + otherGO.name);
                break;
        }
    }

     public virtual void AddToScore()
    {
        ScoreManager.ADD_POINTS(3);
    }
}
