﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Hero : MonoBehaviour
{
    static public Hero S;

    public float speed, rollMult, pitchMult, lengthOfSpeedIncrease;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    private int _numOfWarp = 3;
    private bool _possibleWarp, _speedIncreased = false;
    private float _speedDuration;
    private float _initSpeed;
    public Color red, green, gold, white, none;
    private Color[] availableSkins = new Color[4];
    public int i = 0;


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
        if (_possibleWarp == true && _numOfWarp > 0)
        {
            S.GetComponent<BoundsCheck>().keepOnScreen = false;
            if (transform.position.y > S.GetComponent<BoundsCheck>().camHeight - S.GetComponent<BoundsCheck>().radius)
            {
                pos.y = S.GetComponent<BoundsCheck>().camHeight - S.GetComponent<BoundsCheck>().radius;
                transform.position = pos;
            }

            if (transform.position.y < -S.GetComponent<BoundsCheck>().camHeight + S.GetComponent<BoundsCheck>().radius)
            {
                pos.y = -S.GetComponent<BoundsCheck>().camHeight + S.GetComponent<BoundsCheck>().radius;
                transform.position = pos;
            }

            if (S.GetComponent<BoundsCheck>() != null && S.GetComponent<BoundsCheck>().offLeft)
            {
                pos.x = (transform.position.x * -1) - 1.5f;
                transform.position = pos;
                _numOfWarp--;
                ScoreManager.WARPS--;
            }
            else if (S.GetComponent<BoundsCheck>() != null && S.GetComponent<BoundsCheck>().offRight)
            {
                pos.x = (transform.position.x * -1) + 1.5f;
                transform.position = pos;
                _numOfWarp--;
                ScoreManager.WARPS--;
            }
        }
        else
        {
            _possibleWarp = false;
            S.GetComponent<BoundsCheck>().keepOnScreen = true;

        }

        if (_speedIncreased && lengthOfSpeedIncrease > 0)
        {
            lengthOfSpeedIncrease -= Time.deltaTime;
        }
        else if (_speedIncreased)
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
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        // print("Triggered: " + go.name);

        if (go == _lastTriggerGo)
        {
            return;
        }

        _lastTriggerGo = go;
        if (go.tag == "Enemy") //if the shield was triggered by an enemy
        {                       //Decrease the level of the shield by 1
            shieldLevel--;      //... and Destroy the enemy
            Destroy(go);
        }
        else if (go.tag == "PowerUp")
        {
            //If shield was triggered by power-up
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
                _possibleWarp = true;
                _numOfWarp = 3;
                ScoreManager.WARPS= 3;
                break;

            case WeaponType.shield:
                shieldLevel += Random.Range(1, 4 - shieldLevel);
                break;
            case WeaponType.boost:
                if (speed == _initSpeed)
                {
                    speed *= 2;
                }
                lengthOfSpeedIncrease = _speedDuration;
                _speedIncreased = true;

                break;
            default:
                break;
        }
        //NEED TO ACTUALLY PUT SHIT INTO HERE !!!!! !! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! __ ! _!_! _! !_!_

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
                //Tell Main.S to restart the game after a delay
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }

    private void unlockSkins(){
        availableSkins[0] = white;
        if(ScoreManager._HIGH_SCORE>=10)
        {
            availableSkins[1] = red;
            if (ScoreManager._HIGH_SCORE >= 25){
                availableSkins[2] = green;
                if (ScoreManager._HIGH_SCORE >= 50){
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

        if(Input.GetKeyUp(KeyCode.X)){
            if(i>=3 || availableSkins[i+1] == none)
            {
                i = 0;
                transform.Find("Wing").gameObject.GetComponent<Renderer>().material.color = availableSkins[i];
            }
            else{

                transform.Find("Wing").gameObject.GetComponent<Renderer>().material.color = availableSkins[i+1];
                i++;
            }

        }
    }
}
