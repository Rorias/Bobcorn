using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

[Serializable]

public class GameSaveManager
{
    #region Singleton
    private static GameSaveManager _instance = null;
    private static readonly object padlock = new object();

    private GameSaveManager() { }

    public static GameSaveManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (null == _instance)
                {
                    _instance = new GameSaveManager();
                    LoadSaveGames();
                }
                return _instance;
            }
        }
    }
    #endregion

    public static string savesPath;
    public static List<GameSave> savedGames = new List<GameSave>();

    private static void LoadSaveGames()
    {
        if (!Directory.Exists(savesPath))
        {
            Directory.CreateDirectory(savesPath);
        }

        string[] saves = Directory.GetFiles(savesPath);

        for (int i = 0; i < saves.Length; i++)
        {
            string json = File.ReadAllText(saves[i]);
            GameSave game = (GameSave)JsonUtility.FromJson(json, typeof(GameSave));
            game.LoadGame();
            savedGames.Add(game);
        }
    }
}

