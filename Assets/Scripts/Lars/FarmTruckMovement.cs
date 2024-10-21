using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class FarmTruckMovement : MonoBehaviour
{
    public List<Material> slideMats = new List<Material>();

    public float scrollSpeed;

    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {
        for (int i = 0; i < slideMats.Count; i++)
        {
            slideMats[i].mainTextureOffset += new Vector2(scrollSpeed, 0);
        }
    }
}

