using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SoundEffectPlayer : MonoBehaviour ,  ISelectHandler
{
    public enum Trigger
    {
        None,
        UI_ButtonClick,
        UI_ToggleActive,
        UI_ToggleDisable,
        UI_Toggle,
        UI_InputFieldActive,
    }

    public enum PlayType
    {
        Default, 
    }

    [SerializeField] private bool isEnableOverlapPlay = true;
    public PlayType playType;
    [SerializeField] private Trigger playTrigger = Trigger.None;
    [HideInInspector] public AudioManager.SF_Default defaultSF = AudioManager.SF_Default.NONE; 

    SFAudioController currentSFPlayer;
    AudioManager audioManager;
    int targetSF;

    // Start is called before the first frame update
    void Start()
    {
        Initalize();
    }

    private void OnDestroy()
    {
        Remove();
    }

    void Remove()
    {

    }

    void Initalize()
    {

        switch (playType)
        {
            case PlayType.Default:
                targetSF = (int)defaultSF;
                break; 
        }
         

        switch (playTrigger)
        {
            case Trigger.None:
                break;
            case Trigger.UI_Toggle:
            case Trigger.UI_ToggleDisable:
            case Trigger.UI_ToggleActive: 
                var toggle = gameObject.GetComponent<Toggle>();
                if (toggle)
                    toggle.onValueChanged.AddListener(OnToggle);
                break;
            case Trigger.UI_ButtonClick:
                var button = gameObject.GetComponent<Button>();
                if(button)
                    button.onClick.AddListener(OnClick);
                break; 
        }
    }

    void OnClick()
    {
        PlayAudio();
    }

    void OnToggle(bool isOn)
    {
        switch (playTrigger)
        {
            case Trigger.UI_Toggle:
                PlayAudio();
                break;
            case Trigger.UI_ToggleDisable:
                if (!isOn)
                    PlayAudio();
                break;
            case Trigger.UI_ToggleActive:
                if (isOn)
                    PlayAudio();
                break;
        }
    }

    public virtual void PlayAudio()
    {
        if(!audioManager)
            audioManager = AudioManager.Instance;
        if (!isEnableOverlapPlay && currentSFPlayer && currentSFPlayer.IsPlaying)
            return;
        currentSFPlayer = audioManager.PlayEffectAudio(targetSF);
    }


    public void OnSelect(BaseEventData eventData)
    {
        switch (playTrigger)
        {
            case Trigger.UI_InputFieldActive:
                PlayAudio();
                break;
        }
    }
}
