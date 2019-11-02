using UnityEngine.Audio;
using UnityEngine;

//Pour que la classe soit affichée dans l'inspector
[System.Serializable]
public class Sound
{
    //Classe son
    public string name;
    public AudioClip clip;
    //Slider
    [Range(0f,1f)]
    public float volume;
    [Range(.1f,3f)]
    public float pitch;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
}
