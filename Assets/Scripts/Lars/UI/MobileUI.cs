using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MobileUI : MonoBehaviour
{
    public GameObject cameraUI;
    public GameObject movementUI;
    public GameObject actionUI;

    private RectTransform movementPanel;
    private RectTransform movePad;
    private RectTransform movePadSlider;

    private void Start()
    {
        if (GameManager.onPC)
        {
            cameraUI.SetActive(false);
            movementUI.SetActive(false);
            actionUI.SetActive(false);
        }
    }

    private void Update()
    {
        
    }
}

