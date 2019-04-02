using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracks : MonoBehaviour
{
    public GameObject audio;
    public AudioSource ad;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject go = Instantiate(audio) as GameObject;
        ad = go.GetComponent<AudioSource>();
        PlayAudio();

       
    }

    // Update is called once per frame
    void Update()
    {


    }

    void PlayAudio(){

        ad.Play();
    }
}
