using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    static public LevelManager S;
    private int _level = 0;
    public Text levelText;
    public float bossHealth=0;
    public float bossFireRate=0;


    void Awake()
    {
        if (S == null)
        {
            S = this; // Set the private singleton 
        }
        else
        {
            Debug.LogError("ERROR: LevelManager.Awake(): S is already set!");
        }

        AddLevel();

    }
    public void DelayedDisableLevel()
    {
        //Invoke the DisableLevel() method in 2 seconds
        Invoke("DisableLevel", 2);
    }

    public void DisableLevel()
    {
        levelText.enabled = false; // hides the Level text
    }

    static public void RESTART_LEVEL()
    {
        LEVEL = 0; //Resets the level when the hero dies to 0
    }

    static public void ADD_LEVEL()
    {
        S.AddLevel();
    }

    void AddLevel()
    {
        _level++; // adds the level and then displays the given information

        if (_level == 1)
            levelText.text = "Level " + LEVEL.ToString();
        else
            levelText.text = "Level " + LEVEL.ToString() + "\n" + "Boss Health: " + bossHealth + "\n" + "Boss Fire Rate: " + bossFireRate;
       
        levelText.enabled = true;// enables the txt for the user to see then calls the disable function
        DelayedDisableLevel();
    }

    static public int LEVEL
    {
        get { return S._level; }
        set { S._level = value; }
    }

}
