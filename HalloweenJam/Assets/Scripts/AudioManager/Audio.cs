using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "AudioManager/Audio", order = 1)]
public class Audio : ScriptableObject
{
    public string clipName;
    public AudioClip clip;
    public bool loop;
    public float volume;
    [System.NonSerialized]
    public AudioSource source;
}
