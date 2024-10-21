using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlaceTransitioning : MonoBehaviour
{
    public GameObject player;
    private CharacterController playerCC;
    private Transform teleportPosition;
    public Animator transitionerCanvas;

    private void Start()
    {
        teleportPosition = transform.GetChild(0).gameObject.transform;
        player = GameObject.FindGameObjectWithTag("Player");
        playerCC = player.GetComponent<CharacterController>();
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
        playerCC.enabled = false;
        yield return new WaitForSeconds(1.2f);
        player.transform.position = teleportPosition.position;
        yield return new WaitForSeconds(1f);
        playerCC.enabled = true;
    }
}
