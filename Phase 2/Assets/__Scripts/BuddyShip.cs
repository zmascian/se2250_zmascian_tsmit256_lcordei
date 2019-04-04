using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BuddyShip : MonoBehaviour
{

    public GameObject hero;
    private Hero _heroScript;
    public float posXFromHero;
    private BoundsCheck _bndCheck;
    public float speed, rollMult, pitchMult;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    private bool _possibleWarp, _speedIncreased = false;
    private float _speedDuration;
    private float _initSpeed;
    private Weapon _weapon;


    // This variable holds a reference to the last trigging game object
    private GameObject _lastTriggerGo = null;

    //Declare a new delegate type WeaponFireDelegate 
    public delegate void WeaponFireDelegate();
    //create a WeaponFireDelegate field named fireDelegate.
    public WeaponFireDelegate fireDelegate;

    void Awake()
    {
        _initSpeed = speed;
        _possibleWarp = true;

        _bndCheck = GetComponent<BoundsCheck>();
        _heroScript = hero.GetComponent<Hero>();

        _weapon = gameObject.GetComponentInChildren<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hero == null) //once hero is destroyed, the buddy ship should be destroyed as well
        {
            Destroy(this.gameObject);
        }

        //Need to update speed to always match Hero speed 
        //(may change when Hero colour changes or when hero picks up boost)
        speed = _heroScript.speed;

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
        Vector3 pos = transform.position;

        //if hero is not at right or left edge, move BuddyShip. 
        //This will stop Buddyships from moving when Hero is not
        if (hero!= null && hero.transform.position.x < _bndCheck.camWidth - hero.GetComponent<BoundsCheck>().radius
            && hero.transform.position.x > -_bndCheck.camWidth + hero.GetComponent<BoundsCheck>().radius) 
        {
            pos.x += xAxis * speed * Time.deltaTime; 
        }
        pos.y += yAxis * speed * Time.deltaTime;


        //If hero is too close to buddyShip or too far away...
        if (hero != null && (posXFromHero > Mathf.Abs(transform.position.x - hero.transform.position.x)
            || (posXFromHero < Mathf.Abs(transform.position.x - hero.transform.position.x) && _bndCheck.camWidth - Mathf.Abs(transform.position.x) > 15)))
        {
            //And, if buddyShip is on left side of hero...
            if (transform.position.x - hero.transform.position.x < 0)
            pos.x = hero.transform.position.x - posXFromHero; //Set new pos to be appropriately spaced away

            //Otherwise it will be on the right side
            else
                pos.x = hero.transform.position.x + posXFromHero; //Set new pos to be appropriately spaced away
        }

        transform.position = pos; //Set the gameobjects position to the pos Vector

   
        //use the fireDelegate to fire Weapons
        //First, make sure the button is pressed: Axis ("Jump")
        //Then ensure that fireDelegate isn't null to avoid an error
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }


        if (_possibleWarp == true)
        {
            Warp(pos);
        }
        else
        {
            _possibleWarp = false;
            GetComponent<BoundsCheck>().keepOnScreen = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject otherGO = other.gameObject;
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        if (go == _lastTriggerGo)
        {
            return;
        }

        _lastTriggerGo = go;
        if (other.gameObject.tag == "ProjectileEnemy") //if triggered by an enemy projectile
        {
            Destroy(this.gameObject);      //... and Destroy the enemy
            Destroy(other);
            _lastTriggerGo = other.gameObject;
        }
        else if (go.tag == "EnemyBoss") //if triggered by an enemy
        {
            Destroy(this.gameObject);
            Destroy(go);//... and Destroy the enemy
        }
        else if (go.tag == "Enemy") //if triggered by an enemy
        {                      
            Destroy(this.gameObject);      //... and Destroy the enemy
            Destroy(go);
        }
        else if (go.tag == "PowerUp")
        {
            //If triggered by power-up
            AbsorbPowerUp(go);
        }
        else
        {
            print("Triggered by non-Enemy: " + go.name);
        }
    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        Vector3 pos = transform.position;
        switch (pu.type)
        {
            case WeaponType.warp:
            case WeaponType.shield: 
            case WeaponType.boost:
                _heroScript.AbsorbPowerUp(go);
                break;
            case WeaponType.bomb: //Only destroy the buddyShip, hero should not be hurt
                Destroy(this.gameObject);
                break;
            default:
                break;
        }
        pu.AbsorbedBy();
    }

    void Warp(Vector3 pos)
    {
        GetComponent<BoundsCheck>().keepOnScreen = false;
        if (transform.position.y > GetComponent<BoundsCheck>().camHeight - GetComponent<BoundsCheck>().radius)
        {
            pos.y = GetComponent<BoundsCheck>().camHeight - GetComponent<BoundsCheck>().radius;
            transform.position = pos;
        }

        if (transform.position.y < -GetComponent<BoundsCheck>().camHeight + GetComponent<BoundsCheck>().radius)
        {
            pos.y = -GetComponent<BoundsCheck>().camHeight + GetComponent<BoundsCheck>().radius;
            transform.position = pos;
        }

        if (GetComponent<BoundsCheck>() != null && GetComponent<BoundsCheck>().offLeft)
        {
            pos.x = _bndCheck.camWidth - hero.GetComponent<BoundsCheck>().radius; //adjusts to hero level
            transform.position = pos;
        }
        else if (GetComponent<BoundsCheck>() != null && GetComponent<BoundsCheck>().offRight)
        {
            pos.x = -_bndCheck.camWidth + hero.GetComponent<BoundsCheck>().radius;
            transform.position = pos;
        }
    }

}
