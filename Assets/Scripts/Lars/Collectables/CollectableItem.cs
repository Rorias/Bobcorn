using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(Animator)), RequireComponent(typeof(BoxCollider))]
public class CollectableItem : MonoBehaviour
{
    [HideInInspector] public CollectableData collectable;
    public CollectableSO collectableSO;

    public AudioClip soundEffect;
    public AnimationClip animationEffect;

    private AudioSource audioSource;
    private Animator anim;
    private BoxCollider boxColl;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider>();

        audioSource.dopplerLevel = 0;
        audioSource.playOnAwake = false;

        boxColl.isTrigger = true;
    }

    public void InitializeCollectable(CollectableData _collectable)
    {
        collectable.collectableName = _collectable.collectableName;
        collectable.collected = _collectable.collected;

        if (collectable.collected)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider _coll)
    {
        if (_coll.CompareTag("Player"))
        {
            boxColl.enabled = false;
            collectable.collected = true;

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

