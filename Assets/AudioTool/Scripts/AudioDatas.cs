using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AudioDatas : MonoBehaviour
{


    [HideInInspector] public List<AudioClip> EffectAudios = new List<AudioClip>();

    public AudioClip[] BGMAudios;
    public AudioClip[] SFAudios;
    public GameObject BGMPlayerPrefab;
    public GameObject EffectAudioPrefab;
    public float BGMVolume;
    public float BGMFadeDuration;

    private void Awake()
    {
        EffectAudios.Clear();
        EffectAudios.AddRange(SFAudios);
    }
}
