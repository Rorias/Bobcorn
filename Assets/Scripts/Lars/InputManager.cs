using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public sealed class InputManager
{
    #region Singleton
    private static InputManager instance = null;
    private static readonly object padlock = new object();

    private InputManager() { }

    public static InputManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }
                return instance;
            }
        }
    }
    #endregion

    public enum InputKey
    {
        None,
        Left,
        Forward,
        Right,
        Back,
        Jump,
        Crouch,
        Roll,
        Run,
        Confirm,
        Return,
    };

    public enum KeyType { None, Keyboard, Controller, Mobile, Meta, }

    public enum InputAxis
    {
        None,
        XaxisLeft,
        XaxisRight,
        YaxisForward,
        YaxisBack,
    };

    public enum AxisState { None, Down, Held, Up, }

    public enum PossibleJoystick { Left, Right, }

    public class Key
    {
        public KeyCode code { get; set; }
        public KeyType type { get; set; }
    }

    public class Axis
    {
        public string name { get; set; }
        public bool positive { get; set; }
        public AxisState state { get; set; }
    }

    public Dictionary<InputKey, List<Key>> Inputs = new Dictionary<InputKey, List<Key>>()
    {
        { InputKey.None, new List<Key> {
            new Key() { code = KeyCode.None, type = KeyType.None },
        } },
        { InputKey.Left, new List<Key> {
            new Key() { code = KeyCode.A, type = KeyType.Keyboard },
            new Key() { code = KeyCode.LeftArrow, type = KeyType.Keyboard },
        } },
        { InputKey.Forward, new List<Key> {
            new Key() { code = KeyCode.W, type = KeyType.Keyboard },
            new Key() { code = KeyCode.UpArrow, type = KeyType.Keyboard },
        } },
        { InputKey.Right, new List<Key> {
            new Key() { code = KeyCode.D, type = KeyType.Keyboard },
            new Key() { code = KeyCode.RightArrow, type = KeyType.Keyboard },
        } },
        { InputKey.Back, new List<Key> {
            new Key() { code = KeyCode.S, type = KeyType.Keyboard },
            new Key() { code = KeyCode.DownArrow, type = KeyType.Keyboard },
        } },
        { InputKey.Jump, new List<Key> {
            new Key() { code = KeyCode.Space, type = KeyType.Keyboard },
            new Key() { code = KeyCode.JoystickButton1/*O*/, type = KeyType.Controller },
        } },
        { InputKey.Crouch, new List<Key> {
            new Key() { code = KeyCode.LeftControl, type = KeyType.Keyboard },
            new Key() { code = KeyCode.RightControl, type = KeyType.Keyboard },
            new Key() { code = KeyCode.JoystickButton0/*X*/, type = KeyType.Controller },
        } },
        { InputKey.Roll, new List<Key> {
            new Key() { code = KeyCode.LeftShift, type = KeyType.Keyboard },
            new Key() { code = KeyCode.RightShift, type = KeyType.Keyboard },
            new Key() { code = KeyCode.JoystickButton1/*O*/, type = KeyType.Controller },
        } },
        { InputKey.Run, new List<Key> {
            new Key() { code = KeyCode.LeftShift, type = KeyType.Keyboard },
            new Key() { code = KeyCode.RightShift, type = KeyType.Keyboard },
            new Key() { code = KeyCode.JoystickButton8/*L3*/, type = KeyType.Controller },
        } },
        { InputKey.Confirm, new List<Key> {
            new Key() { code = KeyCode.Return, type = KeyType.Meta },
            new Key() { code = KeyCode.JoystickButton7, type = KeyType.Meta },
        } },
        { InputKey.Return, new List<Key> {
            new Key() { code = KeyCode.Escape, type = KeyType.Meta },
            new Key() { code = KeyCode.Backspace, type = KeyType.Meta },
            new Key() { code = KeyCode.JoystickButton6, type = KeyType.Meta },
        } },
    };

    public Dictionary<InputAxis, List<Axis>> Axiss = new Dictionary<InputAxis, List<Axis>>()
    {
        { InputAxis.XaxisLeft, new List<Axis> {
            new Axis() { state = AxisState.None, name = "JoystickLeftStickX", positive = false },
            new Axis() { state = AxisState.None, name = "JoystickRightStickX", positive = false },
            new Axis() { state = AxisState.None, name = "DpadX", positive = false }, }
        },
        { InputAxis.XaxisRight, new List<Axis> {
            new Axis() { state = AxisState.None, name = "JoystickLeftStickX", positive = true },
            new Axis() { state = AxisState.None, name = "JoystickRightStickX", positive = true },
            new Axis() { state = AxisState.None, name = "DpadX", positive = true }, }
        },
        { InputAxis.YaxisForward, new List<Axis> {
            new Axis() { state = AxisState.None, name = "JoystickLeftStickY", positive = true },
            new Axis() { state = AxisState.None, name = "JoystickRightStickY", positive = true },
            new Axis() { state = AxisState.None, name = "DpadY", positive = true }, }
        },
        { InputAxis.YaxisBack, new List<Axis> {
            new Axis() { state = AxisState.None, name = "JoystickLeftStickY", positive = false },
            new Axis() { state = AxisState.None, name = "JoystickRightStickY", positive = false },
            new Axis() { state = AxisState.None, name = "DpadY", positive = false }, }
        },
    };


    public readonly Dictionary<InputKey, KeyCode> DefaultKeys = new Dictionary<InputKey, KeyCode>()
    {
        { InputKey.Left, KeyCode.A },
        { InputKey.Forward, KeyCode.W },
        { InputKey.Right, KeyCode.D },
        { InputKey.Back, KeyCode.S },
        { InputKey.Jump, KeyCode.Space },
        { InputKey.Crouch, KeyCode.LeftControl },
        { InputKey.Roll, KeyCode.LeftShift },
        { InputKey.Run, KeyCode.LeftShift },
    };

    public readonly Dictionary<InputKey, KeyCode> DefaultButtons = new Dictionary<InputKey, KeyCode>()
    {
        { InputKey.Jump, KeyCode.JoystickButton1 },
        { InputKey.Crouch, KeyCode.JoystickButton0 },
        { InputKey.Roll, KeyCode.JoystickButton1 },
        { InputKey.Run, KeyCode.JoystickButton8 },
    };

    private List<RaycastResult> rayResults = new List<RaycastResult>();

    public bool controllerConnected = false;
    public readonly float deadzone = 0.05f;
    public readonly float switchzone = 0.5f;
    public PossibleJoystick activeJoystick;
    public InputKey lastKey;

    public void UpdateAxis()
    {
        if (controllerConnected)
        {
            foreach (KeyValuePair<InputAxis, List<Axis>> entry in Axiss)
            {
                for (int i = 0; i < Axiss[entry.Key].Count; i++)
                {
                    if ((Input.GetAxis(Axiss[entry.Key][i].name) >= switchzone && Axiss[entry.Key][i].positive) || (Input.GetAxis(Axiss[entry.Key][i].name) <= -switchzone && !Axiss[entry.Key][i].positive))
                    {
                        if (Axiss[entry.Key][i].state == AxisState.None)
                        {
                            Axiss[entry.Key][i].state = AxisState.Down;
                        }
                        else if (Axiss[entry.Key][i].state == AxisState.Down)
                        {
                            Axiss[entry.Key][i].state = AxisState.Held;
                        }
                    }

                    if ((Input.GetAxis(Axiss[entry.Key][i].name) < switchzone && Axiss[entry.Key][i].positive) || (Input.GetAxis(Axiss[entry.Key][i].name) > -switchzone && !Axiss[entry.Key][i].positive))
                    {
                        if (Axiss[entry.Key][i].state == AxisState.Held)
                        {
                            Axiss[entry.Key][i].state = AxisState.Up;
                        }
                        else if (Axiss[entry.Key][i].state == AxisState.Up)
                        {
                            Axiss[entry.Key][i].state = AxisState.None;
                        }
                    }
                }
            }
        }
    }

    public float GetAxisPosition(InputAxis _axis)
    {
        if (controllerConnected)
        {
            for (int i = 0; i < Axiss[_axis].Count; i++)
            {
                if (Input.GetAxis(Axiss[_axis][i].name) > deadzone || Input.GetAxis(Axiss[_axis][i].name) < -deadzone)
                {
                    return Input.GetAxis(Axiss[_axis][i].name);
                }
            }
        }

        return 0;
    }

    public bool GetAxisDown(InputAxis _axis)
    {
        if (controllerConnected)
        {
            for (int i = 0; i < Axiss[_axis].Count; i++)
            {
                if (Axiss[_axis][i].state == AxisState.Down)
                {
                    bool activeStick = activeJoystick == PossibleJoystick.Left ? !Axiss[_axis][i].name.Contains("JoystickRightStick") : !Axiss[_axis][i].name.Contains("JoystickLeftStick");

                    return activeStick;
                }
            }
        }

        return false;
    }

    public bool GetAxis(InputAxis _axis)
    {
        if (controllerConnected)
        {
            for (int i = 0; i < Axiss[_axis].Count; i++)
            {
                if (Axiss[_axis][i].state == AxisState.Held)
                {
                    bool activeStick = activeJoystick == PossibleJoystick.Left ? !Axiss[_axis][i].name.Contains("JoystickRightStick") : !Axiss[_axis][i].name.Contains("JoystickLeftStick");

                    return activeStick;
                }
            }
        }

        return false;
    }

    public bool GetAxisUp(InputAxis _axis)
    {
        if (controllerConnected)
        {
            for (int i = 0; i < Axiss[_axis].Count; i++)
            {
                if (Axiss[_axis][i].state == AxisState.Up)
                {
                    bool activeStick = activeJoystick == PossibleJoystick.Left ? !Axiss[_axis][i].name.Contains("JoystickRightStick") : !Axiss[_axis][i].name.Contains("JoystickLeftStick");

                    return activeStick;
                }
            }
        }

        return false;
    }

    public bool GetAxisReleased(InputAxis _axis)
    {
        if (controllerConnected)
        {
            for (int i = 0; i < Axiss[_axis].Count; i++)
            {
                if (Axiss[_axis][i].state == AxisState.None)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public string GetTouchDown()
    {
        if (Input.touchCount > 0)
        {
            EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current)
            { position = Input.mousePosition, pointerId = -1 }, rayResults);

            if (rayResults.Count > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    if (touch.phase == TouchPhase.Began)
                    {
                        return rayResults[0].gameObject.name;
                    }
                }
            }
            else
            {
                return "Any";
            }
        }

        return string.Empty;
    }

    public string GetTouch()
    {
        if (Input.touchCount > 0)
        {
            EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current)
            { position = Input.mousePosition, pointerId = -1 }, rayResults);

            if (rayResults.Count > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                    {
                        return rayResults[0].gameObject.name;
                    }
                }
            }
            else
            {
                return "Any";
            }
        }

        return string.Empty;
    }

    public string GetTouchUp()
    {
        if (Input.touchCount > 0)
        {
            EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current)
            { position = Input.mousePosition, pointerId = -1 }, rayResults);

            if (rayResults.Count > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    if (touch.phase == TouchPhase.Ended)
                    {
                        return rayResults[0].gameObject.name;
                    }
                }
            }
            else
            {
                return "Any";
            }
        }

        return string.Empty;
    }

    public bool GetKeyDown(InputKey _key)
    {
        for (int i = 0; i < Inputs[_key].Count; i++)
        {
            if (Input.GetKeyDown(Inputs[_key][i].code))
            {
                return true;
            }
        }

        return false;
    }

    public bool GetKey(InputKey _key)
    {
        for (int i = 0; i < Inputs[_key].Count; i++)
        {
            if (Input.GetKey(Inputs[_key][i].code))
            {
                return true;
            }
        }

        return false;
    }

    public bool GetKeyUp(InputKey _key)
    {
        for (int i = 0; i < Inputs[_key].Count; i++)
        {
            if (Input.GetKeyUp(Inputs[_key][i].code))
            {
                return true;
            }
        }

        return false;
    }

    public bool GetKeyTouched(InputKey _key)
    {
        for (int i = 0; i < Inputs[_key].Count; i++)
        {
            if (Input.GetKeyDown(Inputs[_key][i].code) || Input.GetKey(Inputs[_key][i].code) || Input.GetKeyUp(Inputs[_key][i].code))
            {
                return true;
            }
        }

        return false;
    }

    public void LoadInputs(GameSettings _settings)
    {
        Inputs[InputKey.Left].Find(x => x.type == KeyType.Keyboard).code = _settings.left;
        Inputs[InputKey.Forward].Find(x => x.type == KeyType.Keyboard).code = _settings.forward;
        Inputs[InputKey.Right].Find(x => x.type == KeyType.Keyboard).code = _settings.right;
        Inputs[InputKey.Back].Find(x => x.type == KeyType.Keyboard).code = _settings.back;
        Inputs[InputKey.Jump].Find(x => x.type == KeyType.Keyboard).code = _settings.jump;
        Inputs[InputKey.Crouch].Find(x => x.type == KeyType.Keyboard).code = _settings.crouch;
        Inputs[InputKey.Roll].Find(x => x.type == KeyType.Keyboard).code = _settings.roll;
        Inputs[InputKey.Run].Find(x => x.type == KeyType.Keyboard).code = _settings.run;

        Inputs[InputKey.Jump].Find(x => x.type == KeyType.Controller).code = _settings.jumpJoy;
        Inputs[InputKey.Crouch].Find(x => x.type == KeyType.Controller).code = _settings.crouchJoy;
        Inputs[InputKey.Roll].Find(x => x.type == KeyType.Controller).code = _settings.rollJoy;
        Inputs[InputKey.Run].Find(x => x.type == KeyType.Controller).code = _settings.runJoy;

        Inputs[InputKey.Confirm].AddRange(Inputs[InputKey.Jump]);

        activeJoystick = _settings.activeJoystick;

        #region Test code to show that the key gets added as a reference and is thus updated
        /*Debug.Log(Inputs3[InputKey.LeftMenu][Inputs3[InputKey.LeftMenu].Count - 1].code + " REF");
          Debug.Log(Inputs3[InputKey.Left][0].code + " ORIGINAL");

          Key entry = Inputs3[InputKey.Left].First(x => x.type == KeyType.Keyboard);
          entry.code = KeyCode.LeftWindows;

          Debug.Log("CHANGED: ?");
          Debug.Log(Inputs3[InputKey.LeftMenu][Inputs3[InputKey.LeftMenu].Count - 1].code + " REF");
          Debug.Log(Inputs3[InputKey.Left][0].code + " ORIGINAL");
        */
        /*Debug.Log(Inputs3[InputKey.Confirm][Inputs3[InputKey.Confirm].Count - 1].code + " REF");
        Debug.Log(Inputs3[InputKey.Confirm][Inputs3[InputKey.Confirm].Count - 2].code + " REF");
        Debug.Log(Inputs3[InputKey.Fist][0].code + " ORIGINAL");
        Debug.Log(Inputs3[InputKey.Fist][1].code + " ORIGINAL");

        Key entry = Inputs3[InputKey.Fist].First(x => x.type == KeyType.Keyboard);
        entry.code = KeyCode.LeftWindows;

        Debug.Log("CHANGED: ?");
        Debug.Log(Inputs3[InputKey.Confirm][Inputs3[InputKey.Confirm].Count - 1].code + " REF");
        Debug.Log(Inputs3[InputKey.Confirm][Inputs3[InputKey.Confirm].Count - 2].code + " REF");
        Debug.Log(Inputs3[InputKey.Fist][0].code + " ORIGINAL");
        Debug.Log(Inputs3[InputKey.Fist][1].code + " ORIGINAL");
        */
        #endregion
    }

    public void SaveInputs(GameSettings _settings)
    {
        _settings.left = Inputs[InputKey.Left].Find(x => x.type == KeyType.Keyboard).code;
        _settings.forward = Inputs[InputKey.Forward].Find(x => x.type == KeyType.Keyboard).code;
        _settings.right = Inputs[InputKey.Right].Find(x => x.type == KeyType.Keyboard).code;
        _settings.back = Inputs[InputKey.Back].Find(x => x.type == KeyType.Keyboard).code;
        _settings.jump = Inputs[InputKey.Jump].Find(x => x.type == KeyType.Keyboard).code;
        _settings.crouch = Inputs[InputKey.Crouch].Find(x => x.type == KeyType.Keyboard).code;
        _settings.roll = Inputs[InputKey.Roll].Find(x => x.type == KeyType.Keyboard).code;
        _settings.run = Inputs[InputKey.Run].Find(x => x.type == KeyType.Keyboard).code;

        _settings.jumpJoy = Inputs[InputKey.Jump].Find(x => x.type == KeyType.Controller).code;
        _settings.crouchJoy = Inputs[InputKey.Crouch].Find(x => x.type == KeyType.Controller).code;
        _settings.rollJoy = Inputs[InputKey.Roll].Find(x => x.type == KeyType.Controller).code;
        _settings.runJoy = Inputs[InputKey.Run].Find(x => x.type == KeyType.Controller).code;
        _settings.activeJoystick = activeJoystick;
    }
}
