using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameSave
{
    public GameSave() { }
    public GameSave(int _gameID)
    {
        gameID = _gameID;
        gameName = "BobPopcorn";

        collectables = new List<CollectableData>();

        for (int i = 0; i < GameManager.Instance.collectables.Count; i++)
        {
            collectables.Add(new CollectableData() { collectableName = GameManager.Instance.collectables[i].collectableName });
        }
    }

    public int gameID;
    public string gameName;

    public List<CollectableData> collectables = new List<CollectableData>();

    public void SaveGame()
    {
        File.WriteAllText(GameSaveManager.savesPath + gameName + gameID + ".json", JsonUtility.ToJson(this));

        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
    }

    public void LoadGame()
    {
        for (int i = 0; i < collectables.Count; i++)
        {
            //collectables[i].LoadData();
        }
    }
}

