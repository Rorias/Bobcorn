using UnityEngine;

public class ConsoleGUIDebug : MonoBehaviour
{
    static string myLog = "";
    private string output;
    private string stack;
    private string prevOutput;

    private bool active = false;

    private void Awake()
    {
        Application.logMessageReceivedThreaded += Log;
    }

    private void OnDisable()
    {
        Application.logMessageReceivedThreaded -= Log;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            active = !active;
        }
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        output = logString;
        if (output != prevOutput)
        {
            stack = stackTrace;

            if (string.IsNullOrWhiteSpace(stack))
            {
                myLog = output + "\n" + myLog;
            }
            else
            {
                myLog = output + " STACKTRACE: " + stack + "\n" + myLog;
            }

            if (myLog.Length > 10000)
            {
                myLog = myLog.Substring(0, 9000);
            }

            prevOutput = output;
        }
    }

    private void OnGUI()
    {
        if (active) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
        {
            myLog = GUI.TextArea(new Rect(Screen.width / 20f, Screen.height / 20f, Screen.width - (Screen.width / 20f), Screen.height - (Screen.height / 20f)), myLog);
        }
    }
}
