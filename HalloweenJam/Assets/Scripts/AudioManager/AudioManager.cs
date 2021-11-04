using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static AudioManager instance;
    Audio[] audios;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audios = Resources.LoadAll<Audio>("Audios");
            foreach (Audio a in audios)
            {
                a.source = gameObject.AddComponent<AudioSource>();
                a.source.clip = a.clip;
                a.source.volume = a.volume;
                a.source.loop = a.loop;
            }
        }
    }

    public static void Play(string _name)
    {
        foreach (Audio a in instance.audios)
        {
            if(_name == a.clipName)
            {
                a.source.Play();
                return;
            }
        }
    }
    public static void Stop(string _name)
    {
        foreach (Audio a in instance.audios)
        {
            if (_name == a.clipName)
            {
                a.source.Stop();
                return;
            }
        }
    }
}
