using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class UISizer : MonoBehaviour
{
    public float percentFromTop;
    public float percentFromBottom;

    private RectTransform uiRect;
    private RectTransform rect;

    private void Awake()
    {
        uiRect = transform.parent.GetComponent<RectTransform>();
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        float top = (uiRect.sizeDelta.y / 100f) * percentFromTop;
        float bottom = (uiRect.sizeDelta.y / 100f) * percentFromBottom;

        rect.offsetMax = new Vector2(rect.offsetMax.x, -top);
        rect.offsetMin = new Vector2(rect.offsetMin.x, bottom);
    }
}

