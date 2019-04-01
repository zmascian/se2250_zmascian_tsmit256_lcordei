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
        def = Main.GetWeaponDefinition(_type);
        collarRend.material.color = def.color;
        if (gameObject.transform.root.tag == "Enemy")
            def.delayBetweenShots = 1f;
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
            case WeaponType.simple:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;

            case WeaponType.blaster:
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
        }
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if(transform.parent.gameObject.tag == "Hero")
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
        lastShotTime = Time.time;
        return (p);
    }


    // Used to switch weapons
   void Update()
    {
        if (Input.GetKeyDown("c") && gameObject.transform.root.tag == "Hero")
        {
            if (type == WeaponType.simple)
                type = WeaponType.blaster;
            else
                type = WeaponType.simple;
        }
    }
}
