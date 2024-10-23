using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(Animator)), RequireComponent(typeof(BoxCollider))]
public class CollectableItem : MonoBehaviour
{
    [NonSerialized] public CollectableData collectable;
    public CollectableSO collectableSO;

    public AudioClip soundEffect;

    private GameManager gameManager;

    private AudioSource audioSource;
    private Animator anim;
    private BoxCollider boxColl;

    private void Awake()
    {
        gameManager = GameManager.Instance;

        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider>();

        audioSource.dopplerLevel = 0;
        audioSource.playOnAwake = false;

        anim.applyRootMotion = true;
        anim.Play("Collectable", 0, 0);

        boxColl.isTrigger = true;
    }

    public void InitializeCollectable(CollectableData _collectable)
    {
        collectable = _collectable;

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

            if (soundEffect != null)
            {
                audioSource.clip = soundEffect;
                audioSource.Play();
            }

            Collect();
            gameManager.SaveGame();
            StartCoroutine(ICollected());
        }
    }

    private void Collect()
    {
        if (collectable.recollectable)
        {
            collectable.collectionCount++;
            return;
        }

        collectable.collected = true;
    }

    private IEnumerator ICollected()
    {
        anim.Play("Collected", 0, 0);
        yield return new WaitForEndOfFrame();

        while (anim.GetCurrentAnimatorStateInfo(0).IsName("Collected") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}

