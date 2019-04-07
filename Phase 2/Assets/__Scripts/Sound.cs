using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound // stores a sound clip and uses the following variables to edit it
{
    public string name;
    public AudioClip clip;

    [Range(0f,1f)] // range allows for a slider for ease of use
    public float volume;

    [Range(.1f,3f)]
    public float pitch;

    public bool loop; 

    [HideInInspector] // hides this property from the users view
    public AudioSource source; // gives each sound clip and Audio source component
}
