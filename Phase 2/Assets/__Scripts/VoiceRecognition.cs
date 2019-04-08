using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;

public class VoiceRecognition : MonoBehaviour
{
    private KeywordRecognizer _keywordRecognizer;
    private Dictionary<string, Action> _actions = new Dictionary<string, Action>();
    private Main _main;
    public GameObject weaponGO;
    public GameObject buddyShipPrefab;
    private Weapon _weapon;
    private GameObject []_buddyShip;
    private GameObject newBuddyShip;

    // Start is called before the first frame update
    void Start()
    {
        _main = GetComponent<Main>();
        _weapon = weaponGO.GetComponent<Weapon>();

        //Different phrases all increase enemy spawn rate
        _actions.Add("more enemies", IncreaseEnemySpawn);
        _actions.Add("harder", IncreaseEnemySpawn);

        //Different phrases all decrease enemy spawn rate
        _actions.Add("less enemies", DecreaseEnemySpawn);
        _actions.Add("fewer enemies", DecreaseEnemySpawn);
        _actions.Add("easier", DecreaseEnemySpawn);

        //Different phrases all cause fire wave of bullets
        _actions.Add("Jose help", FireBulletWave);
        _actions.Add("help", FireBulletWave);

        //Ability to add another buddy Ship
        _actions.Add("Buy Buddy Ship", AddBuddyShip);
        _actions.Add("Buddy Ship", AddBuddyShip);
        _actions.Add("Buy Friend", AddBuddyShip);

        _keywordRecognizer = new KeywordRecognizer(_actions.Keys.ToArray());
        _keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        _keywordRecognizer.Start();
    }

    //This is a built-in function from UnityEngine.Windows.Speech which recognizes when phrases have been said
    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        _actions[speech.text].Invoke();
    }

    //Increases the spawn rate of enemies by 0.75
    void IncreaseEnemySpawn()
    {
        _main.enemySpawnPerSecond += 0.75f;
        FindObjectOfType<SoundManager>().Play("MoreEnemies");
    }

    //Decreases the spawn rate of enemies by 0.75
    void DecreaseEnemySpawn()
    {
        
        if (_main.enemySpawnPerSecond > 0.75f)
            _main.enemySpawnPerSecond -= 0.75f;

        //This ensures that there will always be at least a very small amount of enemies spawn
        else
            _main.enemySpawnPerSecond = 0.1f;
        FindObjectOfType<SoundManager>().Play("LessEnemies");
    }

    //Fires the sonic weapon with 31 bullets from the weapon script
    void FireBulletWave()
    {
        if (ScoreManager.SCORE >= 5) //Can only use this function if score is large enough
        {
            WeaponType temp = _weapon.type; //Stores the type of weapon the player was using before this sonic
            _weapon.type = WeaponType.sonic;
            _weapon.Fire();
            _weapon.type = temp; //Gives the user the same type of weapon it had before this sonic shot
            ScoreManager.LOSE_POINTS(5);    //Costs points to use the Jose function
        }
        else
        {
            FindObjectOfType<SoundManager>().Play("NoJose");
        }

    }

    void AddBuddyShip()
    {
        _buddyShip = GameObject.FindGameObjectsWithTag("BuddyShip"); //get list of buddyShips currently in scene
        Vector3 displacement = new Vector3(7, 0, 0); //This is the distance the buddyShips are away from hero
        
        if(ScoreManager.SCORE < 25) {
//YUP            //Play unavailable voice sound
            return;
        }

        //If there are two buddy ships
        switch (_buddyShip.Length)
        {
            case 0:
                //If no buddy ships, add a right buddy ship by default
                newBuddyShip = Instantiate(buddyShipPrefab) as GameObject;
                newBuddyShip.name = "RightBuddyShip";
                newBuddyShip.transform.position = Hero.S.transform.position + displacement;

                ScoreManager.LOSE_POINTS(25); //costs 25 points to buy a buddy ship
//YUP                //Play buddy ship added voice sound

                break;

            case 1:
                //If there is a right buddy ship, add a left buddy ship
                if (_buddyShip[0].name == "RightBuddyShip")
                {
                    newBuddyShip = Instantiate(buddyShipPrefab) as GameObject;
                    newBuddyShip.name = "LeftBuddyShip";
                    newBuddyShip.transform.position = Hero.S.transform.position - displacement;
                }
                else //add a right buddy ship
                {
                    newBuddyShip = Instantiate(buddyShipPrefab) as GameObject;
                    newBuddyShip.name = "RightBuddyShip";
                    newBuddyShip.transform.position = Hero.S.transform.position + displacement;
                }

                ScoreManager.LOSE_POINTS(25); //costs 25 points to buy a buddy ship
//YUP                //Play buddy ship added voice sound

                break;

            case 2:
///YUP             //Play unavailable voice sound
                break; //There cannot be more than two buddy ships
        }

    }
}
