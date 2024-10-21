using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

[Serializable]
public sealed class GameSettings
{
    #region Singleton
    private static GameSettings _instance = null;
    private static readonly object padlock = new object();

    private GameSettings() { }

    public static GameSettings Instance
    {
        get
        {
            lock (padlock)
            {
                if (null == _instance)
                {
                    LoadSettings();
                }
                return _instance;
            }
        }
    }
    #endregion

    public static string file;
    public const string fileName = "MegagroeiSettings";

    //music & sound volume settings when quit/started
    public bool firstLoad;
    public float musicVolume;
    public float soundVolume;

    public int hatColor;

    public InputManager.PossibleJoystick activeJoystick;

    public KeyCode left;
    public KeyCode forward;
    public KeyCode right;
    public KeyCode back;
    public KeyCode jump;
    public KeyCode crouch;
    public KeyCode roll;
    public KeyCode run;

    public KeyCode jumpJoy;
    public KeyCode crouchJoy;
    public KeyCode rollJoy;
    public KeyCode runJoy;

    public void SaveSettings()
    {
        File.WriteAllText(file, JsonUtility.ToJson(Instance, true));

        string json = JsonUtility.ToJson(Instance);
        Debug.Log(json);
    }

    private static void LoadSettings()
    {
        if (File.Exists(file))
        {
            string json = File.ReadAllText(file);
            _instance = (GameSettings)JsonUtility.FromJson(json, typeof(GameSettings));
        }
        else
        {
            string json = CreateSettings(file);
            _instance = (GameSettings)JsonUtility.FromJson(json, typeof(GameSettings));
        }
    }

    private static string CreateSettings(string _path)
    {
        if (_path != null)
        {
            File.WriteAllText(_path,
@"{    
    ""firstLoad"": true,
    ""musicVolume"": 0.65,
    ""soundVolume"": 0.4,
    ""activeJoystick"": 0
}");

            return File.ReadAllText(_path);
        }

        return string.Empty;
    }
}

