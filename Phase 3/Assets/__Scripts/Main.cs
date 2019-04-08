using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Enum of various possible types and includes 'shield' type to allow a shield power-up
public enum WeaponType
{
    none,       //no weapon
    simple,     //A simple blaster
    blaster,     //two shots simultaneously
    shield,      //raise sheildLevel
    warp,        //Allows hero to break boundaries.
    boost,       //Increases Speed of hero
    bomb,           //Destorys hero Entry 
    sonic,           //shoots 31 bullets
    enemy,           //enemy's weapon similar to blaster
    missile     //Seeks out enemies
}

public class Main : MonoBehaviour
{
    public static Main S; // A singleton for Main
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    public GameObject prefabBoss;
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond;
    public float enemyDefaultPadding;
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency;
    

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

    public void Start()
    {
        FindObjectOfType<SoundManager>().Play("trackAudio");
        FindObjectOfType<SoundManager>().Play("NewGame");
    }

    public void ShipDestroyed(Enemy e)
    {
        //Maybe generate a powerup
        if(Random.value <= e.powerUpDropChance)
        {
            //Choose which powerup to pick and pick from the possibilities in powerupFrequency array
            int ndx = Random.Range(0, powerUpFrequency.Length); //this creates a random index
            WeaponType puType = powerUpFrequency[ndx];
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            pu.SetType(puType); //set the powerup to be the proper type that was chosen

            pu.transform.position = e.transform.position; //sets the position of powerup to ships position
        }
    }


    public void SpawnEnemy() {
         
       
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
        ScoreManager.RESTART_SCORE();
        //Reload _Scene_0 to restart the game
        SceneManager.LoadScene("_MenuScene");
        
        
    }

    //Static Function that gets a WeaponDefintion from the WEAP_DICT static prrotected field of the Main class
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        //Ensures key exists
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }

        return (new WeaponDefinition());
    }



}
