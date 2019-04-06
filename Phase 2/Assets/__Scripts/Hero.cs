using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Hero : MonoBehaviour
{
    static public Hero S; //Singleton for Hero Script

    public float speed, rollMult, pitchMult, lengthOfSpeedIncrease;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    private int _numOfWarp = 3;
    private bool _possibleWarp, _speedIncreased = false;
    private float _speedDuration;
    private float _initSpeed;
    public Color red, green, gold, white, none; //colours to be used to change hero ship wings
    private Color[] availableSkins = new Color[4];
    public int ndx = 0; //index of availableSkins array
    private float _numOfColChanges = 1.0f; //Number of times the user changes their skin



    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;  // Remeber the underscore

    // This variable holds a reference to the last trigging game object
    private GameObject _lastTriggerGo = null;

    //Declare a new delegate type WeaponFireDelegate 
    public delegate void WeaponFireDelegate();
    //create a WeaponFireDelegate field named fireDelegate.
    public WeaponFireDelegate fireDelegate;



    void Awake()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
        _speedDuration = lengthOfSpeedIncrease;
        _initSpeed = speed;

    }

    // Update is called once per frame
    void Update()
    {

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        //use the fireDelegate to fire Weapons
        //First, make sure the button is pressed: Axis ("Jump")
        //Then ensure that fireDelegate isn't null to avoid an error
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }

        //If it is possible to warp, then provide ability to warp
        if (S != null && _possibleWarp == true && _numOfWarp > 0)
        {
            Warp(pos);
        }
        else //otherwise, ensure that hero does not warp
        {
            _possibleWarp = false;
            S.GetComponent<BoundsCheck>().keepOnScreen = true;
        }

        if (_speedIncreased && lengthOfSpeedIncrease > 0)
        {
            lengthOfSpeedIncrease -= Time.deltaTime; //decrease length of boost if it is currently boosted
        }
        else if (_speedIncreased) //If it is boosted but time is up, return speed to regular
        {
            lengthOfSpeedIncrease = _speedDuration;
            speed = speed / 2;
            _speedIncreased = false;

        }
        unlockSkins();
        changeSkin();

        
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject otherGO = other.gameObject;
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        // print("Triggered: " + go.name);

        if (go == _lastTriggerGo)
        {
            return;
        }

        _lastTriggerGo = go;
        if (other.gameObject.tag == "ProjectileEnemy") //if the shield was triggered by an enemy projectile
        {                
            //Decrease the level of the shield by 1
            shieldLevel--;      //... and Destroy the enemy
            FindObjectOfType<SoundManager>().Play("ShieldBreak");
            Destroy(other.gameObject);
            _lastTriggerGo = other.gameObject;
        }
        else if (go.tag == "EnemyBoss") //if the shield was triggered by an enemy 
        {
            shieldLevel = -1;
            FindObjectOfType<SoundManager>().Play("friendyDestroyAudio");
            Destroy(go);//... and Destroy the enemy
            
        }
        else if (go.tag == "Enemy") //if the shield was triggered by an enemy
        {                       //Decrease the level of the shield by 1
            shieldLevel--;      //... and Destroy the enemy
            FindObjectOfType<SoundManager>().Play("ShieldBreak");
            Destroy(go);
        }
        else if (go.tag == "PowerUp")
        {
            //If shield was triggered by power-up
            AbsorbPowerUp(go);
        }
    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        Vector3 pos = transform.position;
        switch (pu.type)
        {
            //If powerup is warp, set numOfPowerups to be 3 on screen and for variable and let possibleWarp occur
            case WeaponType.warp:
                _possibleWarp = true;
                _numOfWarp = 3;
                ScoreManager.WARPS= 3;
                FindObjectOfType<SoundManager>().Play("Warp");
                break;

            //If powerup is shield, add a random level of shield between 1 and the max possible addition
            case WeaponType.shield:
                shieldLevel += Random.Range(1, 4 - shieldLevel);
                FindObjectOfType<SoundManager>().Play("ShieldUp");
                break;

            //If powerup is boost, multiply speed by two and set the speed duration
            case WeaponType.boost:
                if (speed == _initSpeed)
                {
                    speed *= 2;
                }
                lengthOfSpeedIncrease = _speedDuration;
                _speedIncreased = true;
                FindObjectOfType<SoundManager>().Play("Boost");
                break;
            
             //If powerup is bomb, completely destroy the hero
            case WeaponType.bomb:
                shieldLevel = -1;
                FindObjectOfType<SoundManager>().Play("friendyDestroyAudio");
                break;
            default:
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }


    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            //if the shield is going to be set to less than zero
            if (value < 0)
            {
                Destroy(this.gameObject);
                FindObjectOfType<SoundManager>().Play("friendyDestroyAudio");
                //Tell Main.S to restart the game after a delay
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }

    void unlockSkins(){
        availableSkins[0] = white;
        if(ScoreManager._HIGH_SCORE>=50)
        {
            availableSkins[1] = red;
            if (ScoreManager._HIGH_SCORE >= 200){
                availableSkins[2] = green;
                if (ScoreManager._HIGH_SCORE >= 500){
                    availableSkins[3] = gold;
                }
                else
                {
                    availableSkins[3] = none;
                }
            }
            else
            {
                availableSkins[2] = none;
            }
        }
        else{
            availableSkins[1] = none;
        }

    }

    public void changeSkin(){



        if (Input.GetKeyUp(KeyCode.X)&& !_speedIncreased){

            if (ndx>=3 || availableSkins[ndx+1] == none)
            {
                ndx = 0;
                transform.Find("Wing").gameObject.GetComponent<Renderer>().material.color = availableSkins[ndx];
            }
            else{

                transform.Find("Wing").gameObject.GetComponent<Renderer>().material.color = availableSkins[ndx+1];
                ndx++;
            }
            if(transform.Find("Wing").gameObject.GetComponent<Renderer>().material.color == red){
                _numOfColChanges *= 1.2f;
                speed *= 1.2f;
                _initSpeed *= 1.2f;

            }
            else if(transform.Find("Wing").gameObject.GetComponent<Renderer>().material.color == green) {
                _numOfColChanges *= 1.2f;
                speed *= 1.2f;
                _initSpeed *= 1.2f;

            }
            else if (transform.Find("Wing").gameObject.GetComponent<Renderer>().material.color == gold)
            {
                _numOfColChanges *= 1.2f;
                speed *= 1.2f;
                _initSpeed *= 1.2f;
            }
            else if (transform.Find("Wing").gameObject.GetComponent<Renderer>().material.color == white)
            {
                speed = speed/_numOfColChanges;
                _initSpeed = _initSpeed / _numOfColChanges;
                _numOfColChanges = 1.0f;

            }

        }
    }

    void Warp(Vector3 pos)
    {
        S.GetComponent<BoundsCheck>().keepOnScreen = false;

        //Although keepOnScreen is false, this will still keep buddyShip on screen vertically
        if (transform.position.y > S.GetComponent<BoundsCheck>().camHeight - S.GetComponent<BoundsCheck>().radius)
        {
            pos.y = S.GetComponent<BoundsCheck>().camHeight - S.GetComponent<BoundsCheck>().radius;
            transform.position = pos;
        }

        //Although keepOnScreen is false, this will still keep buddyShip on screen vertically
        if (transform.position.y < -S.GetComponent<BoundsCheck>().camHeight + S.GetComponent<BoundsCheck>().radius)
        {
            pos.y = -S.GetComponent<BoundsCheck>().camHeight + S.GetComponent<BoundsCheck>().radius;
            transform.position = pos;
        }

        //If it went off left, then put hero on the other side of screen
        if (S.GetComponent<BoundsCheck>() != null && S.GetComponent<BoundsCheck>().offLeft)
        {
            pos.x = S.GetComponent<BoundsCheck>().camWidth - S.GetComponent<BoundsCheck>().radius;
            transform.position = pos;
            _numOfWarp--;
            ScoreManager.WARPS--;
        }
        //If it went off right, then put hero on the other side of screen
        else if (S.GetComponent<BoundsCheck>() != null && S.GetComponent<BoundsCheck>().offRight)
        {
            pos.x = -S.GetComponent<BoundsCheck>().camWidth + S.GetComponent<BoundsCheck>().radius;
            transform.position = pos;
            _numOfWarp--;
            ScoreManager.WARPS--;
        }
    }
}
