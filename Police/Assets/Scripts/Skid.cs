using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class Skid : MonoBehaviour
{
    public bool skidding { get; private set; }
    public bool PlayingAudio { get; private set; }
    private AudioSource AudioSource;
    private Transform SkidTrail;

    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        AudioSource.loop = true;
        PlayingAudio = false;
    }

    public void PlayAudio()
    {
        AudioSource.Play();
        PlayingAudio = true;
    }


    public void StopAudio()
    {
        AudioSource.Stop();
        PlayingAudio = false;
    }
}
