using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    static private ScoreManager S;
    public GUIText highScore;
    public GUIText currScore;
    static public int _HIGH_SCORE = 0;
    private int _score = 0;
    public Enemy enemy1;
    public Enemy enemy2;
    public Enemy enemy3;

    void Awake()
    {
        if (S == null)
        {                                        // c
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
    }


    void Start()
    {
        currScore.text = _score.ToString();
        highScore.text = _HIGH_SCORE.ToString();
    }


    static public void ADDSCORE(Collider other)
    {
        S.AddScore(other);
    }

    public void AddScore(Collider other)
    {
        if (other.gameObject == enemy1)
            _score++;
        else if (other.gameObject == enemy2)
            _score += 2;
        else if (other.gameObject == enemy3)
            _score += 3;

        currScore.text = _score.ToString();

    }


    static public void RESTARTSCORE()
    {
        S.RestartScore();
    }

     public void RestartScore()
    {
        if (_HIGH_SCORE <= _score)
        {
            _HIGH_SCORE = _score;
            PlayerPrefs.SetInt("HeroHighScore", _score);
            highScore.text = PlayerPrefs.GetInt("HeroHighScore").ToString();
        }
    }

}
