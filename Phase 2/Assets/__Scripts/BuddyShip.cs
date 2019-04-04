using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BuddyShip : MonoBehaviour
{

    public GameObject hero;
    private Hero _heroScript;
    public float posXFromHero;          //A standard value that buddyShip should be from Hero
    private BoundsCheck _bndCheck;      
    public float speed, rollMult, pitchMult;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    private bool _possibleWarp;
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
        //Fires the weapon script attached to weapon GO of buddyShip
        _weapon.Fire();

        if(hero == null) //once hero is destroyed, the buddy ship should be destroyed as well
        {
            Destroy(this.gameObject);
        }

        //Need to update speed to always match Hero speed 
        //It may change when Hero colour changes or when hero picks up boost
        speed = _heroScript.speed;

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
        Vector3 pos = transform.position;

        //if hero is not at right or left edge, move BuddyShip. 
        //This will keep Buddyships from moving when Hero is not
        if (hero!= null && hero.transform.position.x < _bndCheck.camWidth - hero.GetComponent<BoundsCheck>().radius
            && hero.transform.position.x > -_bndCheck.camWidth + hero.GetComponent<BoundsCheck>().radius) 
        {
            pos.x += xAxis * speed * Time.deltaTime; 
        }
        pos.y += yAxis * speed * Time.deltaTime;


        //If hero is too close to buddyShip or too far away...
        if (hero != null && (posXFromHero > Mathf.Abs(transform.position.x - hero.transform.position.x) //If it is too close
            || (posXFromHero < Mathf.Abs(transform.position.x - hero.transform.position.x) && //If it is too far
            _bndCheck.camWidth - Mathf.Abs(transform.position.x) > 15))) //If the buddyShip isn't warped on the otherside
        {
            //And, if buddyShip is on left side of hero...
            if (transform.position.x - hero.transform.position.x < 0)
            pos.x = hero.transform.position.x - posXFromHero; //Set new pos to be appropriately spaced away

            //Otherwise it will be on the right side
            else
                pos.x = hero.transform.position.x + posXFromHero; //Set new pos to be appropriately spaced away
        }

        transform.position = pos; //Set the gameobjects position to the pos Vector


        if (_possibleWarp == true)
        {
            Warp(pos);
        }
        else
        {
            _possibleWarp = false;
            GetComponent<BoundsCheck>().keepOnScreen = true; //keep the go on screen if not warping
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        if (other.gameObject.tag == "ProjectileEnemy") //if triggered by an enemy projectile
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
            _lastTriggerGo = other.gameObject;
        }
        if (go == _lastTriggerGo)   //if its the same as last time, don't do anything
        {
            return;
        }
        _lastTriggerGo = go;
        if (go.tag == "EnemyBoss") //if triggered by an enemy
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

    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        Vector3 pos = transform.position;
        switch (pu.type)
        {
            //Warp, shield, boost powerups can all be shared directly with hero and do not affect buddyShip
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

    //This takes care of the buddyShip warp, 
    //Unlike hero, buddyShip can warp an infinite amount of times
    void Warp(Vector3 pos)
    {
        GetComponent<BoundsCheck>().keepOnScreen = false;

        //Although keepOnScreen is false, this will still keep buddyShip on screen vertically
        if (transform.position.y > GetComponent<BoundsCheck>().camHeight - GetComponent<BoundsCheck>().radius)
        {
            pos.y = GetComponent<BoundsCheck>().camHeight - GetComponent<BoundsCheck>().radius;
            transform.position = pos;
        }

        //Although keepOnScreen is false, this will still keep buddyShip on screen vertically
        if (transform.position.y < -GetComponent<BoundsCheck>().camHeight + GetComponent<BoundsCheck>().radius)
        {
            pos.y = -GetComponent<BoundsCheck>().camHeight + GetComponent<BoundsCheck>().radius;
            transform.position = pos;
        }

        //If it went off left, then put buddyship on the other side of screen
        if (GetComponent<BoundsCheck>() != null && GetComponent<BoundsCheck>().offLeft)
        {
            pos.x = _bndCheck.camWidth - hero.GetComponent<BoundsCheck>().radius; //positions heroRadius from edge
            transform.position = pos;
        }

        //If it went off right, then put buddyship on the other side of screen
        else if (GetComponent<BoundsCheck>() != null && GetComponent<BoundsCheck>().offRight)
        {
            pos.x = -_bndCheck.camWidth + hero.GetComponent<BoundsCheck>().radius; //positions heroRadius from edge
            transform.position = pos;
        }
    }

}
