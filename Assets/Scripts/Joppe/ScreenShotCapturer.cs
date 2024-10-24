using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotCapturer : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CaptureScreenshot();
        }
    }

    private void CaptureScreenshot()
    {
        string name = "SS" + Time.realtimeSinceStartup + ".png";
        ScreenCapture.CaptureScreenshot(name,2);
        Debug.Log("Screenshot " + name + " made!");
    }
}
