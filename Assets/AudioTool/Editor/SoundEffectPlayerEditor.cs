using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundEffectPlayer))]
public class SoundEffectPlayerEditor : Editor
{
    public SoundEffectPlayer.PlayType op;

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        var myScript = target as SoundEffectPlayer;
        string paramName = "";
        switch (myScript.playType)
        {
            case SoundEffectPlayer.PlayType.Default:
                paramName = "TargetSF " + myScript.defaultSF.GetType().Name;
                myScript.defaultSF = (AudioManager.SF_Default)EditorGUILayout.EnumPopup(paramName, myScript.defaultSF);

                break; 
        }
        //if (myScript.flag)
        //    myScript.i = EditorGUILayout.IntSlider("I field:", myScript.i, 1, 100);

    }
}
