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

    // Start is called before the first frame update
    void Start()
    {
        _main = GetComponent<Main>();

        _actions.Add("more enemies", IncreaseEnemySpawn);
        _actions.Add("less enemies", DecreaseEnemySpawn);
        _actions.Add("fewer enemies", DecreaseEnemySpawn);
        _actions.Add("Jose help", FireBulletWave);

        _keywordRecognizer = new KeywordRecognizer(_actions.Keys.ToArray());
        _keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        _keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        _actions[speech.text].Invoke();
    }

    void IncreaseEnemySpawn()
    {
        _main.enemySpawnPerSecond += 0.5f;
    }

    void DecreaseEnemySpawn()
    {
        //This ensures that there will always be at least a very small amount of enemies spawn
        if (_main.enemySpawnPerSecond > 0.5f)
            _main.enemySpawnPerSecond -= 0.5f;
        else
            _main.enemySpawnPerSecond = 0.1f;
    }

    void FireBulletWave()
    {

    }
}