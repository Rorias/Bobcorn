using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class FarmTruckMovement : MonoBehaviour
{
    public List<GameObject> roadPieces = new List<GameObject>();
    public List<Material> slideMats = new List<Material>();

    public float scrollSpeed;
    public float roadbumpSpeed;

    private List<Vector3> originalPositions = new List<Vector3>();

    private Vector3 randomOffset;

    private bool moveToRandomVector = true;

    private void Start()
    {
        for (int i = 0; i < roadPieces.Count; i++)
        {
            originalPositions.Add(roadPieces[i].transform.position);
        }
    }

    private void Update()
    {
        if (moveToRandomVector)
        {
            randomOffset = new Vector3(0, UnityEngine.Random.Range(-3, 4), 0);
            moveToRandomVector = false;
        }

        for (int i = 0; i < roadPieces.Count; i++)
        {
            roadPieces[i].transform.position = Vector3.MoveTowards(roadPieces[i].transform.position, originalPositions[i] + randomOffset, Time.deltaTime * roadbumpSpeed);

            if (roadPieces[i].transform.position == originalPositions[i] + randomOffset)
            {
                moveToRandomVector = true;
            }
        }

        for (int i = 0; i < slideMats.Count; i++)
        {
            slideMats[i].mainTextureOffset += new Vector2(scrollSpeed, 0);
        }
    }
}

