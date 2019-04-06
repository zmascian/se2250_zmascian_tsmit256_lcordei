using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;                       //Letter to show on power-up
    public Color color = Color.white;           //Color of Collar and power-up
    public GameObject projectilePrefab;         //Prefab for projectiles
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;               //Amount of damage cause
    public float continousDamage = 0;           //Damage per second (laser)
    public float delayBetweenShots = 0;
    public float velocity = 20;                 //Speed of projectiles
}

public class Weapon : MonoBehaviour
{
    
    //private  _simpleSound, _blasterSound, _sonicSound, _enemySound, _missileSound;
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime; //Time the last shot was fired
    private Renderer collarRend;

    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        //call setType() for the default _type of WeaponType.none
        SetType(_type);

        //Dynamiccally create an achor for all Projectiles
        if(PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        //Find the fireDelegate of the root gameobject
        GameObject rootGo = transform.root.gameObject;
        if(rootGo.GetComponent<Hero>() != null)
        {
            rootGo.GetComponent<Hero>().fireDelegate += Fire;
        }

    }

    

    public WeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }

    public void SetType(WeaponType wt)
    {
        _type = wt;
        if(type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_type); //get the weapond definition from the main script for type
        collarRend.material.color = def.color;
        if (gameObject.transform.root.tag == "EnemyBoss")  // sets the fire rate of the Enemy Boss's weapon
            def.delayBetweenShots = 1/(gameObject.transform.root.GetComponent<Enemy_3>().fireRate*0.5f);
        lastShotTime = 0; //You can fire immediately after _type is set
    }

    public void Fire()
    {
        //If this.gameObject is inactive, return
        if (!gameObject.activeInHierarchy) return;
        //If it hasn't been enough time between shots, return
        if(Time.time - lastShotTime < def.delayBetweenShots)
        {
            return;
        }
        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        if(transform.up.y < 0)
        {
            vel.y = -vel.y;
        }
        switch (type)
        {
            //If simple, then just shoot one bullet straight up
            case WeaponType.simple:
               
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;

            //Three bullets
            case WeaponType.blaster:
                gameObject.transform.root.GetComponent<SoundManager>().Invoke("Play", 0);
                p = MakeProjectile();   //Middle Proj.
                p.rigid.velocity = vel;
                p = MakeProjectile();   //Right Proj.
                p.transform.rotation = Quaternion.AngleAxis(30, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile();   //Left Proj.
                p.transform.rotation = Quaternion.AngleAxis(-30, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;

            case WeaponType.sonic: //Makes 31 bullets
                gameObject.transform.root.GetComponent<SoundManager>().Invoke("Play(missileAudio)", 0);
                p = MakeProjectile();   //Middle Proj.
                p.rigid.velocity = vel;

                for (int i = 0; i < 15; i++) //loop for each of the 15 symetric bullets on either side of middle bullet
                {
                    p = MakeProjectile();   //Right Proj.
                    p.transform.rotation = Quaternion.AngleAxis(2.5f * i, Vector3.back);
                    p.rigid.velocity = p.transform.rotation * vel;
                    p = MakeProjectile();   //Left Proj.
                    p.transform.rotation = Quaternion.AngleAxis(-2.5f * i, Vector3.back);
                    p.rigid.velocity = p.transform.rotation * vel;
                }
                break;

            case WeaponType.enemy:

                p = MakeProjectile();   //Middle Proj.
                p.rigid.velocity = vel;
                p = MakeProjectile();   //Right Proj.
                p.transform.rotation = Quaternion.AngleAxis(20, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile();   //Left Proj.
                p.transform.rotation = Quaternion.AngleAxis(-20, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                
                break;
            
             //Just make one bullet and let MissileProjectile control homing
            case WeaponType.missile:
               
                p = MakeProjectile();
                break;

            default:
                break;
        }
    }

    public Projectile MakeProjectile()
    {
        //Instantiate from given prefab
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if(transform.parent.gameObject.tag == "Hero") //If hero, make its tags projectileHero
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else if(transform.parent.gameObject.tag == "BuddyShip") //make tags ProjectileHero for buddyships as well
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time; //Set the time of last shot time to be now
        return (p);
    }


    // Used to switch weapons
   void Update()
    {
        //If c is pressed, switch from simle to blaster or vice versa
        if (Input.GetKeyDown("c") && gameObject.transform.root.tag == "Hero")
        {
            if (type == WeaponType.simple)
                type = WeaponType.blaster;
            else
                type = WeaponType.simple;
        }
    }
}
