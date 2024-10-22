using Cinemachine;

using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class StaticCamAngle : MonoBehaviour
{
    public static bool staticCamera;

    public Transform point;

    private CinemachineVirtualCamera vCam;
    private Cinemachine3rdPersonFollow vCamFollow;

    private Transform oldFollow;
    private float vertLength;
    private float camDistance;

    private void Awake()
    {
        vCam = FindObjectOfType<CinemachineVirtualCamera>();
        vCamFollow = FindObjectOfType<Cinemachine3rdPersonFollow>();
    }

    private void OnTriggerEnter(Collider _coll)
    {
        oldFollow = vCam.Follow;
        vertLength = vCamFollow.VerticalArmLength;
        camDistance = vCamFollow.CameraDistance;

        vCam.Follow = point;
        vCamFollow.VerticalArmLength = 0;
        vCamFollow.CameraDistance = 0;

        staticCamera = true;
    }

    private void OnTriggerExit(Collider _coll)
    {
        vCam.Follow = oldFollow;
        vCamFollow.VerticalArmLength = vertLength;
        vCamFollow.CameraDistance = camDistance;

        staticCamera = false;
    }
}

