using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region singleton
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                Debug.LogError("Called too early");
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        Application.targetFrameRate = 60;

        GameSettings.file = Application.persistentDataPath + "/" + GameSettings.fileName + ".json";

        gameSettings = GameSettings.Instance;
        input = InputManager.Instance;
        onPC = SystemInfo.deviceType == DeviceType.Desktop;

        LoadSettings();

        if (Input.GetJoystickNames().Length > 0 && !string.IsNullOrWhiteSpace(Input.GetJoystickNames()[0]))
        {
            input.controllerConnected = true;
        }

        SceneManager.sceneLoaded += FauxAwake;

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public static bool onPC;

    private GameSettings gameSettings;
    private InputManager input;

    private void FauxAwake(Scene _s, LoadSceneMode _lsm)
    {
        if (gameSettings.firstLoad)
        {
            //Do initialization
            foreach (KeyValuePair<InputManager.InputKey, KeyCode> Default in input.DefaultKeys)
            {
                InputManager.Key key = input.Inputs[Default.Key].First(x => x.type == InputManager.KeyType.Keyboard);
                key.code = input.DefaultKeys[Default.Key];
            }

            foreach (KeyValuePair<InputManager.InputKey, KeyCode> Default in input.DefaultButtons)
            {
                InputManager.Key key = input.Inputs[Default.Key].First(x => x.type == InputManager.KeyType.Controller);
                key.code = input.DefaultButtons[Default.Key];
            }

            gameSettings.firstLoad = false;
            SaveInputs();
        }
    }

    private void Update()
    {
        input.UpdateAxis();
    }

    public void LoadSettings()
    {
        LoadInputs();
    }

    private void LoadInputs()
    {
        input.LoadInputs(gameSettings);
    }

    public void SaveInputs()
    {
        input.SaveInputs(gameSettings);
        gameSettings.SaveSettings();
    }
}
[Flags] public enum CollectableType { None, Popcorn, Ducky, };
