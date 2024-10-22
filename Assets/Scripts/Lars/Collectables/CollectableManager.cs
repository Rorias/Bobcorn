using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class CollectableManager : MonoBehaviour
{
    [HideInInspector] [SerializeField] public List<CollectableData> collectables = new List<CollectableData>();
    private List<CollectableItem> collectableItemsInScene = new List<CollectableItem>();

    private GameManager gameManager;

    private void Awake()
    {
        collectableItemsInScene = FindObjectsByType<CollectableItem>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        InitializeCollectables();
    }

    private void InitializeCollectables()
    {
        for (int i = 0; i < gameManager.collectables.Count; i++)
        {
            CollectableSO thisCollectableSO = gameManager.collectables.First(x => x.collectableName == collectables[i].collectableName);
            CollectableItem thisollectableItem = collectableItemsInScene.FirstOrDefault(x => x.collectableSO.collectableName == thisCollectableSO.collectableName);
            CollectableData thisCollectable = collectables.First(x => x.collectableName == thisCollectableSO.collectableName);

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

