using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CollectableManager : MonoBehaviour
{
    [HideInInspector] [SerializeField] public List<CollectableData> collectables = new List<CollectableData>();
    private List<CollectableItem> collectableItemsInScene = new List<CollectableItem>();

    private GameManager gameManager;

    private void Awake()
    {
        SceneManager.sceneLoaded += FauxAwake;
    }

    private void FauxAwake(Scene _s, LoadSceneMode _lsm)
    {
        collectableItemsInScene = FindObjectsByType<CollectableItem>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();

        InitializeCollectables();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void InitializeCollectables()
    {
        for (int i = 0; i < collectableItemsInScene.Count; i++)
        {
            CollectableItem thisollectableItem = collectableItemsInScene[i];
            CollectableSO thisCollectableSO = gameManager.collectables.First(x => x.collectableName == thisollectableItem.collectableSO.collectableName);
            CollectableData thisCollectable = collectables.First(x => x.collectableName == thisCollectableSO.collectableName);
            thisCollectable.recollectable = thisCollectableSO.recollectable;

            if (thisollectableItem != null)
            {
                thisollectableItem.InitializeCollectable(thisCollectable);
            }
        }
    }

    public void Save(GameSave _saveGame)
    {
        _saveGame.collectables = collectables;
    }

    public void Load(GameSave _saveGame)
    {
        collectables = _saveGame.collectables;
    }
}

