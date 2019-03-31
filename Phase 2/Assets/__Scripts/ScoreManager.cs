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

        scoreText.text = "Score: "+ SCORE.ToString();
        highscoreText.text= "Highscore: " + _HIGH_SCORE.ToString();
        numWarps.text = "Warps: " + _warps.ToString();

      
    }

    static public void RESTART_SCORE() {
        if (_HIGH_SCORE <= SCORE)
        {
            _HIGH_SCORE = SCORE;
            PlayerPrefs.SetInt("HeroHighScore", SCORE);
            
        }
        else
            SCORE = 0;


    }




    static public void ADD_POINTS(int point)
    {
        S.AddPoints(point);
    }

    void AddPoints(int point )
    {
        _score += point;
        scoreText.text = "Score: " + SCORE.ToString();
    }
    


    static public int SCORE {
        get { return S._score; }
        set { S._score = value; }
    }

    static public int WARPS
    {
        get { return S._warps; }
        set { S._warps = value;
            S.numWarps.text = "Warps: " + S._warps.ToString();
        }
    }

}
