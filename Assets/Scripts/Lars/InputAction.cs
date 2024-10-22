using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(Animator)), RequireComponent(typeof(BoxCollider))]
public class InputAction : MonoBehaviour
{
    public AudioClip soundEffect;
    public AnimationClip animationEffect;
    public InputManager.InputKey activationKey;
    public string activationTouch;
    [Space]
    public bool repeatable;

    private InputManager input;

    private AudioSource audioSource;
    private Animator anim;
    private BoxCollider boxColl;

    private bool activated = false;

    private void Awake()
    {
        input = InputManager.Instance;

        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider>();

        audioSource.dopplerLevel = 0;
        audioSource.playOnAwake = false;

        boxColl.isTrigger = true;
    }

    private void OnTriggerStay(Collider _coll)
    {
        if (_coll.CompareTag("Player"))
        {
            if ((input.GetKeyDown(activationKey) || input.GetTouchDown() == activationTouch))
            {
                if (!repeatable && activated)
                {
                    return;
                }

                activated = true;

                if (soundEffect != null)
                {
                    audioSource.clip = soundEffect;
                    audioSource.Play();
                }

                if (animationEffect != null)
                {
                    anim.Play(animationEffect.name, 0, 0);
                }
            }
        }
    }
}

