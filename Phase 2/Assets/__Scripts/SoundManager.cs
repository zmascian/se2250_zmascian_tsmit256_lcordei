using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject audio;
    private Dictionary<string, AudioSource> _audio = new Dictionary<string,AudioSource>();
    // Start is called before the first frame update
    void Start()
    {
        _audio.Add("soundTrackAudio", audio.transform.Find("soundTrackAudio").GetComponent<AudioSource>());
        _audio.Add("enemyAudio", audio.transform.Find("enemyAudio").GetComponent<AudioSource>());
        _audio.Add("missileAudio", audio.transform.Find("missileAudio").GetComponent<AudioSource>());
        _audio.Add("sonicAudio", audio.transform.Find("sonicAudio").GetComponent<AudioSource>());
        _audio.Add("destroyAudio", audio.transform.Find("destroyAudio").GetComponent<AudioSource>());
        _audio.Add("blasterAudio", audio.transform.Find("blasterAudio").GetComponent<AudioSource>());
        _audio.Add("simpleAudio", audio.transform.Find("simpleAudio").GetComponent<AudioSource>());

    }

  

    void Play()
    {
        _audio["blasterAudio"].Play();
    }

}
