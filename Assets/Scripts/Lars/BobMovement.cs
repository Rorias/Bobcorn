using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BobMovement : MonoBehaviour
{
    public RectTransform movementPanel;
    public RectTransform movePad;
    public RectTransform movePadSlider;

    public float movementSpeedMultiplier;

    private enum TouchField { none, action, movement, camera, }

    private List<RaycastResult> rayResults = new List<RaycastResult>();

    private CharacterController cc;

    private Vector2 initialMovePadPos;
    private Vector2 startPos;
    private Vector2 touchPos;
    private TouchField initialField;

    private bool pressed;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current)
        { position = Input.mousePosition, pointerId = -1 }, rayResults);

        if (rayResults.Count > 0)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    startPos = touch.position;
                    touchPos = touch.position;
                    Down();
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    touchPos = touch.position;
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    Up();
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    startPos = Input.mousePosition;
                    touchPos = Input.mousePosition;
                    Down();
                }

                if (Input.GetMouseButton(0))
                {
                    touchPos = Input.mousePosition;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    Up();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (pressed)
        {
            switch (initialField)
            {
                case TouchField.action:
                    break;
                case TouchField.movement:
                    MoveInDirection();
                    break;
                case TouchField.camera:
                    MoveCamera();
                    break;
                case TouchField.none:
                default:
                    break;
            }
        }
    }

    private void Down()
    {
        switch (rayResults[0].gameObject.name)
        {
            case "Camera":
                InitMoveCamera();
                break;
            case "Action":
                break;
            case "Movement":
                InitMoveInDirection();
                break;
            default:
                break;
        }

        Debug.Log(rayResults[0].gameObject.name);
    }

    private void Up()
    {
        pressed = false;
        movePad.gameObject.SetActive(false);
        movePadSlider.gameObject.SetActive(false);
    }

    private void InitMoveCamera()
    {
        pressed = true;
        initialField = TouchField.camera;
    }

    private void InitMoveInDirection()
    {
        pressed = true;
        initialField = TouchField.movement;
        movePad.gameObject.SetActive(true);
        movePadSlider.gameObject.SetActive(true);
        Debug.Log(rayResults[0].screenPosition + " ray");
        Debug.Log(startPos + " start");
        Debug.Log(movePadSlider.anchoredPosition + " moveIcon");
        Debug.Log(movementPanel.sizeDelta);
        Debug.Log(movementPanel.rect.height);

        float moveIconX = -(movementPanel.sizeDelta.x / 2);
        float moveIconY = -(movementPanel.rect.height / 2);
        Vector2 moveIconOffset = new Vector2(moveIconX, moveIconY);
        Debug.Log(moveIconOffset);
        initialMovePadPos = moveIconOffset + startPos;
        movePad.anchoredPosition = initialMovePadPos;
        movePadSlider.anchoredPosition = initialMovePadPos;
    }

    private void MoveCamera()
    {

    }

    private void MoveInDirection()
    {
        Vector2 moveOffset = Vector2.ClampMagnitude(touchPos - startPos, movePad.rect.width / 2);
        movePadSlider.anchoredPosition = initialMovePadPos;
        movePadSlider.anchoredPosition = initialMovePadPos + moveOffset;
        Vector2 moveSpeed = new Vector2(moveOffset.x / (movePad.rect.width / 2), moveOffset.y / (movePad.rect.width / 2));
        cc.SimpleMove(new Vector3(moveSpeed.x, 0, moveSpeed.y) * movementSpeedMultiplier);
    }
}

