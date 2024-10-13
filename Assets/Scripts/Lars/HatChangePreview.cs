using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class HatChangePreview : MonoBehaviour
{
    public Material hat;
    public GameObject ui;
    public Slider slider;

    private GameSettings settings;

    private void Awake()
    {
        settings = GameSettings.Instance;
    }

    private void Start()
    {
        hat.color = new Color(hat.color.r, (settings.hatColor / 255f), hat.color.b, 1);
        slider.value = settings.hatColor;
    }

    private void OnTriggerEnter(Collider _coll)
    {
        if (_coll.CompareTag("Player"))
        {
            ui.SetActive(true);
        }
    }

    public void SliderMove(float _value)
    {
        hat.color = new Color(hat.color.r, (_value / 255f), hat.color.b, 1);
        settings.hatColor = Mathf.RoundToInt(hat.color.g * 255f);
    }

    public void SaveHatValue()
    {
        settings.SaveSettings();
        ui.SetActive(false);
    }
}

