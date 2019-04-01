using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel : MonoBehaviour
{
    public GameObject prefabBoss;
    private Main main;
    private BoundsCheck bndCheck;
    private bool _bossspawned = false;
    private GameObject _boss;
    private bool _levelOverOccured = false;
    private int _pastLevelScore = 0;
    private float _prevEnemySpawnRate;
    private void Awake()
    {
        
        main = GetComponent<Main>();
        bndCheck = GetComponent<BoundsCheck>();
    }

    private void Update()
    {
        if (_bossspawned &&_boss==null && _levelOverOccured==false)
        {
            LevelOver();
            _levelOverOccured = true;
            _bossspawned = false;
              
        }
        if (ScoreManager.SCORE >= _pastLevelScore+30  &&_bossspawned==false)
        {
            
                SpawnBoss();
            
            _pastLevelScore += 30;

        }
    }
    public void SpawnBoss()
    {
        
         _bossspawned = true;
        _levelOverOccured = false;
         _boss = Instantiate<GameObject>(prefabBoss);

        _prevEnemySpawnRate = main.enemySpawnPerSecond;
        main.enemySpawnPerSecond = 0f;
        
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

    }

    public  void LevelOver()
    {
        main.enemySpawnPerSecond = _prevEnemySpawnRate;
        main.SpawnEnemy();
    }
}
