using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class SceneTransitioning : MonoBehaviour
{
    private GameObject player;
    public string sceneName;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
