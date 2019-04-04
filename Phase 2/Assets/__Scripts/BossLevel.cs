using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossLevel : MonoBehaviour
{
    public GameObject prefabBoss;
    private Main _main;
    private BoundsCheck bndCheck;
    private bool _bossspawned = false;
    private GameObject _boss;
    private bool _levelOverOccured = false;
    private int _pastLevelScore = 0;
    private float _prevEnemySpawnRate;


  

    private void Awake()
    {
        _main = GetComponent<Main>();// Obtains a reference to main and boundscheck for later use
        bndCheck = GetComponent<BoundsCheck>();
    }

    private void Update()
    {
        if (_bossspawned &&_boss==null && _levelOverOccured==false) // checks if there is a boss on screen, if it has spawned before and if a level has previously occured
        {
            LevelOver(); // calls the level over function and resets the booleans
            _levelOverOccured = true;
            _bossspawned = false;
              
        }
        if (ScoreManager.SCORE >= _pastLevelScore+30  &&_bossspawned==false) // checks to see if the score is at a certain value and checks if the boss has spawned
        {
            _prevEnemySpawnRate = _main.enemySpawnPerSecond; // gets a copy of the current spawn rate
            _main.enemySpawnPerSecond = 0; //sets the spawn rate to 0
            LevelManager.S.bossFireRate = prefabBoss.GetComponent<Enemy_3>().fireRate* (LevelManager.LEVEL); // gets the boss's fire rate to display
            LevelManager.S.bossHealth = prefabBoss.GetComponent<Enemy_3>().health* (LevelManager.LEVEL);  // gets the Boss's health to display
            LevelManager.ADD_LEVEL(); // adds a level 
            Invoke("SpawnBoss", 7f);
            
            _pastLevelScore += 30;

        }
    }
    public void SpawnBoss()
    {
       // resets all the booleans
        _bossspawned = true;
        _levelOverOccured = false;
        _boss = Instantiate<GameObject>(prefabBoss);

        

        //Position enemy above the screen with random x position
        float enemyPadding = 1.5f;
        if (_boss.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(_boss.GetComponent<BoundsCheck>().radius);
        }

        //Set the initial position for this spawned enemy
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        _boss.transform.position = pos;

        // sets the fire rate and health of the boss
        _boss.GetComponent<Enemy_3>().fireRate += (LevelManager.LEVEL-2) ;
        _boss.GetComponent<Enemy_3>().health *= (LevelManager.LEVEL-1);
        
    }

    public  void LevelOver()
    {
        _main.enemySpawnPerSecond = _prevEnemySpawnRate;  // returns the game to the previous spawnrate
        _main.Invoke("SpawnEnemy", 1); // restarts the spawning waves
    }
}
