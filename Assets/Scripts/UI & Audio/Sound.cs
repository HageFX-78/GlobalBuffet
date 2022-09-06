using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(1.0f, 100.0f)]
    public float volume;
    [Range(1.0f, 10.0f)]
    public float pitch;
    public AudioSource src;
}
