using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening; 

//배경 음악 재생, 효과음 재생 클래스.
public class AudioManager : PersistentSingleton<AudioManager> {

    public enum BGMType_Default
    {
        BGM1, 
        EndNextIndex,

        StartIndex = BGM1,
        EndIndex = EndNextIndex - 1,
    } 


    public enum SF_Default
    {
        Button_01 = 0,
        Common_Capture,
        Common_StartRecord,
        ENDNext,

        NONE = -1,
        START = Button_01,
        END = ENDNext - 1,
    }
     

    AudioSource BGMPlayer;
    GameObject BGMPlayerPrefab => AudioDatas.BGMPlayerPrefab;
    GameObject EffectPlayerPrefab => AudioDatas.EffectAudioPrefab;
    List<SFAudioController> createSFAudios = new List<SFAudioController>();
    private AudioDatas audioDatas = null; 

    public float BGMVolume = 1.0f;
    public float SFVoluem = 0.75f; 
    bool bMuteBGM;
    bool bMuteSF;
    public bool IsPlayingBGM { get
        {
            return GetBGMPlayer().isPlaying && GetBGMPlayer().volume > 0;
        }
    }
    public bool IsPausedBGM { get { return !GetBGMPlayer().isPlaying; } }
    public bool IsMuteBGM { get { return bMuteBGM; } }
    public bool IsMuteSF { get { return bMuteSF; } }
    public AudioDatas AudioDatas
    {
        get
        { 
            if (audioDatas == null)
                audioDatas = FindObjectOfType<AudioDatas>();
            return audioDatas;
        }
    }
     

    private void Awake()
    {
        bMuteBGM = false;
        bMuteSF = false;
        CheckInitalize();
    }

    void CheckInitalize()
    {
        if (AudioDatas != null)
            return;
        if(AudioDatas == null)
            audioDatas = FindObjectOfType<AudioDatas>();
        if (!AudioDatas)
            return; 
        GetBGMPlayer();
        BGMPlayer.volume = BGMVolume;
    }

    AudioSource GetBGMPlayer()
    {
        if (BGMPlayer == null)
        {
            CheckInitalize();
            BGMPlayer = Instantiate(BGMPlayerPrefab).GetComponent<AudioSource>();
            BGMPlayer.volume = BGMVolume;
        }
        return BGMPlayer;
    }

    public SFAudioController PlayEffectAudio(int SFIndex)
    {
        return PlayEffectAudio(SFIndex, null);
    }

    public SFAudioController PlayEffectAudio(AudioClip clip)
    {
        if (bMuteSF)
            return null;
        SFAudioController EffectPlayer = Instantiate(EffectPlayerPrefab).GetComponent<SFAudioController>();
        EffectPlayer.OnEnd += OnEndSF;
        EffectPlayer.PlayAudio(clip, SFVoluem);
        createSFAudios.Add(EffectPlayer);
        return EffectPlayer;
    }

    Action OnAudioEnd_Event;
    public SFAudioController PlayEffectAudio(int SFIndex, Action EndCallback)
    {
        if (bMuteSF)
            return null;

        if (AudioDatas.EffectAudios.Count <= SFIndex)
            return null;

        SFAudioController EffectPlayer = Instantiate(EffectPlayerPrefab).GetComponent<SFAudioController>();
        EffectPlayer.OnEnd += (sf)=>
        {
            OnEndSF(sf);
            EndCallback?.Invoke();
        };
        EffectPlayer.PlayAudio(AudioDatas.EffectAudios[SFIndex], SFVoluem); 

        createSFAudios.Add(EffectPlayer);

        return EffectPlayer;
    }
     
    void OnEndSF(SFAudioController targetSF)
    {
        createSFAudios.Remove(targetSF);
        if (OnAudioEnd_Event != null)
            OnAudioEnd_Event();
        OnAudioEnd_Event = null;
    }

    Tweener BGMFadeOutTween;
    Tweener BGMFadeInTween;
    public void PlayBGM(int bgmType, bool isImmediately = false)
    {
        if(BGMFadeOutTween != null && BGMFadeOutTween.IsPlaying())
        {
            BGMFadeOutTween.Kill();
            if(BGMFadeInTween != null)
                BGMFadeInTween.Kill();
        }
        else if (BGMFadeInTween != null && BGMFadeInTween.IsPlaying())
        {
            BGMFadeOutTween.Kill();
            BGMFadeInTween.Kill();
        }
        float fadeDuration = AudioDatas.BGMFadeDuration;
        if (isImmediately)
            fadeDuration = 0;
        BGMFadeOutTween = GetBGMPlayer().DOFade(0, fadeDuration);
        BGMFadeOutTween.onComplete = () =>
        {
            GetBGMPlayer().clip = AudioDatas.BGMAudios[bgmType];
            GetBGMPlayer().Play();
            BGMFadeInTween = GetBGMPlayer().DOFade(AudioDatas.BGMVolume, fadeDuration);
            BGMFadeInTween.onComplete = () =>
            {

            };
        }; 
    }

    public void StopBGM()
    {
        if (BGMFadeOutTween != null && BGMFadeOutTween.IsPlaying())
        {
            BGMFadeOutTween.Kill();
            if (BGMFadeInTween != null)
                BGMFadeInTween.Kill();
        }
        else if (BGMFadeInTween != null && BGMFadeInTween.IsPlaying())
        {
            BGMFadeOutTween.Kill();
            BGMFadeInTween.Kill();
        }

        BGMFadeOutTween = GetBGMPlayer().DOFade(0, AudioDatas.BGMFadeDuration); 
    }

    public void PauseBGM()
    {
        GetBGMPlayer().Pause();
    }

    public void ResumeBGM()
    {
        GetBGMPlayer().UnPause();
    }

    Tweener BGMVolumeTween;
    public void SetBGMVolume(float Volume)
    {
        if (BGMVolumeTween != null && BGMVolumeTween.IsPlaying())
            BGMVolumeTween.Kill();
        BGMVolumeTween = GetBGMPlayer().DOFade(Volume, 1.0f);
    }

    public void ClearSFs()
    {
        for(int i = 0; i < createSFAudios.Count; i++)
        {
            if(createSFAudios[i])
                createSFAudios[i].Claer();
        }
        createSFAudios.Clear();
    }

    public void SetSFVolume(float Volume)
    {
        SFVoluem = Volume;
    }

    public void SetMuteBGM(bool bMute)
    {
        bMuteBGM = bMute;
        GetBGMPlayer().mute = bMute;
    }

    public void SetMuteSF(bool bMute)
    {
        bMuteSF = bMute; 
    }

}
