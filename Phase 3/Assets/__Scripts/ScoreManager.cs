using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    static public ScoreManager S;
    private int _score = 0;
    private int _warps = 0;
    static public int _HIGH_SCORE;
    public Text scoreText;
    public Text highscoreText;
    public Text numWarps;


    void Awake()
    {
        if (S == null)
        {                                        
            S = this; // Set the private singleton 
        }
        else
        {
            Debug.LogError("ERROR: ScoreManager.Awake(): S is already set!");
        }

        // Check for a high score in PlayerPrefs
        if (PlayerPrefs.HasKey("HeroHighScore"))
        {
            _HIGH_SCORE = PlayerPrefs.GetInt("HeroHighScore");
        }
        //Display the score and highscore when the game starts up
        scoreText.text = "Score: "+ SCORE.ToString();
        highscoreText.text= "Highscore: " + _HIGH_SCORE.ToString();
        numWarps.text = "";
      
    }

    static public void RESTART_SCORE() {
        if (_HIGH_SCORE <= SCORE) // At the end of each game the highscore is checked and replaced if the score was higher
        {
            _HIGH_SCORE = SCORE;
            PlayerPrefs.SetInt("HeroHighScore", SCORE);
            
        }
        else
            SCORE = 0; // if it was not reset the score
    }


    static public void ADD_POINTS(int point)
    {
        S.ChangePoints(point); // adds points to the users score
    }

    void ChangePoints(int point )
    {
        _score += point;
        scoreText.text = "Score: " + SCORE.ToString(); // sets the score according to the points variable passed in
    }

    static public void LOSE_POINTS(int point)
    {
        S.ChangePoints(-point); // takes away points from the users score
    }

    static public int SCORE { // score property to access the score variable
        get { return S._score; }
        set { S._score = value; }
    }

    static public int WARPS // managers the number of warps and its respective counter
    {
        get { return S._warps; }
        set { S._warps = value;
            if (S._warps > 0)
            {
                S.numWarps.text = "Warps: " + S._warps.ToString();
            }
            else{
                S.numWarps.text = "";
            }
        }
    }

}
