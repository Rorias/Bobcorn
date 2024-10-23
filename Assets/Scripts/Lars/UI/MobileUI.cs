using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MobileUI : MonoBehaviour
{
    public GameObject cameraUI;
    public GameObject movementUI;
    public GameObject actionUI;

    public RectTransform movementPanel;
    public RectTransform movePad;
    public RectTransform movePadSlider;

    private Vector2 initialMovePadPos;
    private Vector2 startPos;
    private Vector2 touchPos;

    private Touch activeTouch;
    private int activeTouchIndex = -1;

    private bool pressed;

    private void Start()
    {
        if (GameManager.onPC)
        {
            if (cameraUI != null)
            {
                cameraUI.SetActive(false);
            }
            movementUI.SetActive(false);
            actionUI.SetActive(false);
        }
    }

    public int GetActiveIndex()
    {
        return activeTouchIndex;
    }

    public void OpenMovePad(int _touchId)
    {
        startPos = Input.GetTouch(_touchId).position;
        touchPos = Input.GetTouch(_touchId).position;
        pressed = true;
        //initialField = TouchField.movement;
        movePad.gameObject.SetActive(true);
        movePadSlider.gameObject.SetActive(true);

        float moveIconX = -(movementPanel.sizeDelta.x / 2);
        float moveIconY = -(movementPanel.rect.height / 2);
        Vector2 moveIconOffset = new Vector2(moveIconX, moveIconY);

        initialMovePadPos = moveIconOffset + startPos;
        movePad.anchoredPosition = initialMovePadPos;
        movePadSlider.anchoredPosition = initialMovePadPos;
    }

    public void CloseMovePad()
    {
        pressed = false;
        movePad.gameObject.SetActive(false);
        movePadSlider.gameObject.SetActive(false);
        //activeTouchIndex = -1;
    }

    public Vector2 MoveMovePad(int _touchId)
    {
        if (movePad.gameObject.activeInHierarchy)
        {
            touchPos = Input.GetTouch(_touchId).position;
            Vector2 moveOffset = Vector2.ClampMagnitude(touchPos - startPos, movePad.rect.width / 2);
            movePadSlider.anchoredPosition = initialMovePadPos;
            movePadSlider.anchoredPosition = initialMovePadPos + moveOffset;
            return new Vector2(moveOffset.x / (movePad.rect.width / 2), moveOffset.y / (movePad.rect.width / 2));
        }
        else
        {
            return Vector2.zero;
        }
    }
}

