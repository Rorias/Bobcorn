using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer masterMixer;

    public Vector2 carHigh;
    public Vector2 carLow;
    public Vector2 radioHigh;
    public Vector2 radioLow;
    public float transitionDuration = 1.5f;

    private Coroutine transitionCoroutine;

    public void TransitionToInside()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(SmoothTransition(transitionDuration, true)); // 2 seconds duration
    }

    public void TransitionToOutside()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(SmoothTransition(transitionDuration, false)); // 2 seconds duration
    }

    private IEnumerator SmoothTransition(float duration, bool inside)
    {

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newCarHigh, newCarLow, newRadioHigh, newRadioLow;

            if (inside)
            {
                newCarHigh = Mathf.Lerp(carHigh.x, carHigh.y, elapsedTime / duration);
                newCarLow = Mathf.Lerp(carLow.x, carLow.y, elapsedTime / duration);
                newRadioHigh = Mathf.Lerp(radioHigh.x, radioHigh.y, elapsedTime / duration);
                newRadioLow = Mathf.Lerp(radioLow.x, radioLow.y, elapsedTime / duration);
            }
            else
            {
                newCarHigh = Mathf.Lerp(carHigh.y, carHigh.x, elapsedTime / duration);
                newCarLow = Mathf.Lerp(carLow.y, carLow.x, elapsedTime / duration);
                newRadioHigh = Mathf.Lerp(radioHigh.y, radioHigh.x, elapsedTime / duration);
                newRadioLow = Mathf.Lerp(radioLow.y, radioLow.x, elapsedTime / duration);
            }
            

            // Apply the interpolated values to the AudioMixer
            masterMixer.SetFloat("CarHigh", newCarHigh);
            masterMixer.SetFloat("CarLow", newCarLow);
            masterMixer.SetFloat("RadioHigh", newRadioHigh);
            masterMixer.SetFloat("RadioLow", newRadioLow);

            yield return null; // Wait until the next frame
        }
    }
}
