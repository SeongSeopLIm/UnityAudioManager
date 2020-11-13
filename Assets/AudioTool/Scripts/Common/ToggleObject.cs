using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ToggleObject : MonoBehaviour {
    private Toggle TargetToggle;
    [SerializeField] private GameObject[] ActiveObjects;
    [SerializeField] private GameObject[] DisableObjects;

    private void Awake()
    {
        TargetToggle = gameObject.GetComponent<Toggle>();
        TargetToggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void Start()
    {
        UpdateObjects();
    }

    public void OnValueChanged(bool bNewValue)
    {
        UpdateObjects();
    }

    void UpdateObjects()
    {
        if (TargetToggle.isOn)
        {
            ActiveObjects.ToList().ForEach((iter) => iter.SetActive(true));
            DisableObjects.ToList().ForEach((iter) => iter.SetActive(false));
        }
        else
        {
            DisableObjects.ToList().ForEach((iter) => iter.SetActive(true));
            ActiveObjects.ToList().ForEach((iter) => iter.SetActive(false));

        }
    }

}
