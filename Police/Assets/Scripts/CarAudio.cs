using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
    public AudioClip lowAccelClip; //clip audio z dzwiekiem przyspieszania (niski)
    public AudioClip lowDecelClip; //clip audio z dzwiekiem zwalniania (niski)
    public AudioClip highAccelClip; //clip audio z dzwiekiem przyspieszania (wysoki)
    public AudioClip highDecelClip; //clip audio z dzwiekiem zwalniania (wysoki)
    public float pitchMultiplier = 0.7f; //wykorzystywany w zmianie wysokosci dzwieku audio clip'ow
    public float lowPitchMin = 1f; //minimalna wysokosc dzwieku dla niskich dzwiekow
    public float lowPitchMax = 5f; //maksymalna wysokosc dzwieku dla wysokich dzwiekow
    public float highPitchMultiplier = 0.25f; //wykorzystywany do zmiany wysokosci dzwiekow wysokich
    public float dopplerLevel = 1; //ilosc efektu dopplera wykorzystywanego w dzwieku
    public bool useDoppler = true; //czy wykorzystywac efekt dopplera

    private AudioSource LowAccel; //zrodlo dzwieku przyspieszania (niskiego)
    private AudioSource LowDecel; //zrodlo dzwieku zwalniania (niskiego)
    private AudioSource HighAccel; //zrodlo dzwieku przyspieszania (wysokiego)
    private AudioSource HighDecel; //zrodlo dzwieku zwalniania (wysokiego)
    private bool startedSound; //czy dzwiek jest odtwarzany
    private CarController car; //klasa reprezentujaca samochod

    private void StartSound() {
        car = GetComponent<CarController>();

        HighAccel = SetUpEngineAudioSource(highAccelClip);
        LowAccel = SetUpEngineAudioSource(lowAccelClip);
        LowDecel = SetUpEngineAudioSource(lowDecelClip);
        HighDecel = SetUpEngineAudioSource(highDecelClip);

        startedSound = true;
    }


    public void StopSound() {
        foreach (var source in GetComponents<AudioSource>()) {
            Destroy(source);
        }

        startedSound = false;
    }

    private AudioSource SetUpEngineAudioSource(AudioClip clip) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = 0;
            source.loop = true;

            // start the clip from a random point
            source.time = Random.Range(0f, clip.length);
            source.Play();
            source.minDistance = 5;
            source.dopplerLevel = 0;
            return source;
    }

    private static float ULerp(float from, float to, float value) {
            return (1.0f - value)*from + value*to;
    }

    void Update()
    {
        if(!startedSound) {

            StartSound();
        }
        if(startedSound) {

            float pitch = ULerp(lowPitchMin, lowPitchMax, car.revs);
            pitch = Mathf.Min(lowPitchMax, pitch);

            LowAccel.pitch = pitch*pitchMultiplier;
            LowDecel.pitch = pitch*pitchMultiplier;
            HighAccel.pitch = pitch*highPitchMultiplier*pitchMultiplier;
            HighDecel.pitch = pitch*highPitchMultiplier*pitchMultiplier;

            float accFade = Mathf.Abs(car.accel);
            float decFade = 1 - accFade;

            float highFade = Mathf.InverseLerp(0.2f, 0.8f, car.revs);
            float lowFade = 1 - highFade;

            highFade = 1 - ((1 - highFade)*(1 - highFade));
            lowFade = 1 - ((1 - lowFade)*(1 - lowFade));
            accFade = 1 - ((1 - accFade)*(1 - accFade));
            decFade = 1 - ((1 - decFade)*(1 - decFade));

            LowAccel.volume = lowFade*accFade;
            LowDecel.volume = lowFade*decFade;
            HighAccel.volume = highFade*accFade;
            HighDecel.volume = highFade*decFade;

            HighAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
            LowAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
            HighDecel.dopplerLevel = useDoppler ? dopplerLevel : 0;
            LowDecel.dopplerLevel = useDoppler ? dopplerLevel : 0;


        }
    }
}
