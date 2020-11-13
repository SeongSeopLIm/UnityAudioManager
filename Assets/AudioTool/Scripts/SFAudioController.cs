using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class SFAudioController : MonoBehaviour
{
    public event Action<SFAudioController> OnEnd;
    public bool IsPlaying => audioPlayer.isPlaying;
    AudioSource audioPlayer;
    public AudioSource AudioPlayer => audioPlayer;


    private void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    void Start()
    {
    }

    public void PlayAudio(AudioClip Clip, float Volume)
    {
        audioPlayer.clip = Clip;
        audioPlayer.Play(); 
        Invoke("DestryAudio", Clip.length + 0.2f);

    }
    public void Claer()
    {
        CancelInvoke();
        DestryAudio();
    }

    public void SetAudioVolume(float targetVolume, float duration)
    {
        audioPlayer.DOFade(targetVolume, duration);
    }

    void DestryAudio()
    {
        OnEnd?.Invoke(this);
        Destroy(gameObject);
    }

}
