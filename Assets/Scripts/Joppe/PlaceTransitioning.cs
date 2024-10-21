using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlaceTransitioning : MonoBehaviour
{
    public GameObject player;
    private Transform teleportPosition;
    public Animator transitionerCanvas;

    private void Start()
    {
        teleportPosition = transform.GetChild(0).gameObject.transform;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(TeleportSequence());
        }
    }
    private IEnumerator TeleportSequence()
    {
        transitionerCanvas.Play("PopcornTransition");
        yield return new WaitForSeconds(1.2f);
        player.transform.position = teleportPosition.position;
    }
}
