﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Enum of various possible types and includes 'shield' type to allow a shield power-up
public enum WeaponType
{
    none,       //no weapon
    blaster,    //A simple blaster
    spread,     //two shots simultaneously
    phaser,     //shots that move in waves
    missile,    //homing missiles
    laser,      //Damage over time
    shield      //raise sheildLevel
}

public class Main : MonoBehaviour
{
    public static Main S; // A singleton for Main
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond;
    public float enemyDefaultPadding;
    public WeaponDefinition[] weaponDefinitions;

    private BoundsCheck bndCheck;

    private void Awake()
    {
        S = this;
        //Set bndCheck to reference the BoundsCheck component on this gameObject 
        bndCheck = GetComponent<BoundsCheck>();
        //Invoke SpawnEnemy() once (in 2 seconds, based on default values)
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        //A generic dictionary with WeaponType as the key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }

    }

    public void SpawnEnemy() {
        //Pick a random enemy prefab to instantiate
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        //Position enemy above the screen with random x position
        float enemyPadding = enemyDefaultPadding;
        if(go.GetComponent<BoundsCheck>() != null){
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        //Set the initial position for this spawned enemy
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;

        //Invoke SpawnEnemy() again
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

    }

    public void DelayedRestart (float delay)
    {
        //Invoke the restart() method in delay seconds
        Invoke("Restart", delay);
    }
   
    public void Restart()
    {
        //Reload _Scene_0 to restart the game
        SceneManager.LoadScene("_Scene_0");
    }
}
