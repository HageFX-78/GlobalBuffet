using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [NonReorderable]
    public Sound[] BGM;
    [NonReorderable]
    public Sound[] SoundEffects;
    public static AudioManager amInstance;
    void Awake()
    {
        if (amInstance == null)
        {
            amInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        foreach (Sound s in BGM)
        {
            s.src = gameObject.AddComponent<AudioSource>();
            s.src.clip = s.clip;
            s.src.volume = s.volume;
            s.src.pitch = s.pitch;
        }
        foreach (Sound s in SoundEffects)
        {
            s.src = gameObject.AddComponent<AudioSource>();
            s.src.clip = s.clip;
            s.src.volume = s.volume;
            s.src.pitch = s.pitch;
        }
    }
    public void PlayBGM(string name)
    {
        Sound s = Array.Find(BGM, sound => sound.name == name);
        s.src.loop = true;
        s.src.Play();
    }
    public void PlaySF(string name)
    {
        Sound s = Array.Find(SoundEffects, sound => sound.name == name);
        s.src.Play();
    }
    public void StopBGM(string name)
    {
        Sound s = Array.Find(BGM, sound => sound.name == name);
        s.src.Stop();
    }
    public void StopSF(string name)
    {
        Sound s = Array.Find(SoundEffects, sound => sound.name == name);
        s.src.Stop();
    }
    public void StopAllSF()
    {
        foreach (Sound s in SoundEffects)
        {
            s.src.Stop();
        }
    }

}
