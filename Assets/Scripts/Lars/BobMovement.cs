﻿using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BobMovement : MonoBehaviour
{
    [Tooltip("Speed ​​at which the character moves. It is not affected by gravity or jumping.")]
    public float velocity = 5f;
    [Tooltip("This value is added to the speed value while the character is sprinting.")]
    public float runAddition = 3.5f;
    [Tooltip("This value is added to the speed value while the character is sprinting.")]
    public float rollAddition = 8f;
    [Tooltip("The higher the value, the higher the character will jump.")]
    public float jumpForce = 18f;
    [Tooltip("Stay in the air. The higher the value, the longer the character floats before falling.")]
    public float jumpTime = 0.85f;
    [Space]
    [Tooltip("Force that pulls the player down. Changing this value causes all movement, jumping and falling to be changed as well.")]
    public float gravity = 9.8f;

    public bool crouchToggle;
    public bool runToggle;


    private float jumpElapsedTime = 0;

    // Player states
    private bool isJumping = false;
    private bool isRunning = false;
    private bool isRolling = false;
    private bool isCrouching = false;

    // Inputs
    private float inputHorizontal;
    private float inputVertical;
    private bool inputJump;
    private bool inputCrouch;
    private bool inputRun;
    private bool inputRoll;


    public float movementSpeedMultiplier;

    private InputManager input;

    private List<RaycastResult> rayResults = new List<RaycastResult>();

    private Animator animator;
    private CharacterController cc;

    private Vector2 startPos;
    private Vector2 touchPos;

    private void Awake()
    {
        input = InputManager.Instance;

        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (!GameManager.onPC)
        {
            switch (input.GetTouchDown())
            {
                case "Camera":
                    Debug.Log("pressing camera UI");
                    break;
                case "Action":
                    Debug.Log("pressing action UI");
                    break;
                case "Movement":
                    Debug.Log("pressing movement UI");
                    break;
                case "Any":
                    Debug.Log("pressed outside UI");
                    break;
                default:
                    //Debug.Log("Not on a touchable device");
                    break;
            }
        }
        else
        {
            GetMovementPC();
            Run();
            Roll();
        }
    }

    private void FixedUpdate()
    {
        float velocityAddition = 0;

        if (isRunning)
        {
            velocityAddition = runAddition;
        }

        if (isCrouching)
        {
            velocityAddition = -(velocity * 0.50f); // -50% velocity
        }

        if (isRolling)
        {
            velocityAddition = rollAddition;
        }

        // Direction movement
        float directionX = inputHorizontal * (velocity + velocityAddition) * Time.deltaTime;
        float directionZ = inputVertical * (velocity + velocityAddition) * Time.deltaTime;
        float directionY = 0;

        // Jump handler
        if (isJumping)
        {
            // Apply inertia and smoothness when climbing the jump
            // It is not necessary when descending, as gravity itself will gradually pulls
            directionY = Mathf.SmoothStep(jumpForce, jumpForce * 0.30f, jumpElapsedTime / jumpTime) * Time.deltaTime;

            // Jump timer
            jumpElapsedTime += Time.deltaTime;
            if (jumpElapsedTime >= jumpTime)
            {
                isJumping = false;
                jumpElapsedTime = 0;
            }
        }

        // Add gravity to Y axis
        directionY = directionY - gravity * Time.deltaTime;


        // --- Character rotation --- 
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Relate the front with the Z direction (depth) and right with X (lateral movement)
        forward *= directionZ;
        right *= directionX;

        if (directionX != 0 || directionZ != 0)
        {
            float angle = Mathf.Atan2(forward.x + right.x, forward.z + right.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
        }

        // --- End rotation ---
        Vector3 verticalDirection = Vector3.up * directionY;
        Vector3 horizontalDirection = forward + right;

        Vector3 movement = verticalDirection + horizontalDirection;

        cc.Move(movement);
    }

    private void Walk()
    {

    }

    private void Run()
    {
        if (isRunning)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x + 1, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }

    private void Jump()
    {

    }

    private void Roll()
    {

    }

    private void DodgeRoll()
    {

    }

    private void Crouch()
    {

    }

    private void Crawl()
    {

    }

    private void Up()
    {
        //movePad.gameObject.SetActive(false);
        // movePadSlider.gameObject.SetActive(false);
    }

    private void InitMoveInDirection()
    {
        //pressed = true;
        //initialField = TouchField.movement;
        //movePad.gameObject.SetActive(true);
        //movePadSlider.gameObject.SetActive(true);
        //Debug.Log(rayResults[0].screenPosition + " ray");
        //Debug.Log(startPos + " start");
        //Debug.Log(movePadSlider.anchoredPosition + " moveIcon");
        //Debug.Log(movementPanel.sizeDelta);
        //Debug.Log(movementPanel.rect.height);

        //float moveIconX = -(movementPanel.sizeDelta.x / 2);
        //float moveIconY = -(movementPanel.rect.height / 2);
        //Vector2 moveIconOffset = new Vector2(moveIconX, moveIconY);
        //Debug.Log(moveIconOffset);
        //initialMovePadPos = moveIconOffset + startPos;
        //movePad.anchoredPosition = initialMovePadPos;
        //movePadSlider.anchoredPosition = initialMovePadPos;
    }

    private void MoveInDirection()
    {
        //Vector2 moveOffset = Vector2.ClampMagnitude(touchPos - startPos, movePad.rect.width / 2);
        //movePadSlider.anchoredPosition = initialMovePadPos;
        //movePadSlider.anchoredPosition = initialMovePadPos + moveOffset;
        //Vector2 moveSpeed = new Vector2(moveOffset.x / (movePad.rect.width / 2), moveOffset.y / (movePad.rect.width / 2));
        //cc.SimpleMove(new Vector3(moveSpeed.x, 0, moveSpeed.y) * movementSpeedMultiplier);
    }

    private void GetMovementPC()
    {
        GetHorizontal();
        GetVertical();

        if (crouchToggle)
        {
            inputCrouch = input.GetKeyDown(InputManager.InputKey.Crouch);

            if (inputCrouch)
            {
                isCrouching = !isCrouching;
            }
        }
        else
        {
            isCrouching = input.GetKey(InputManager.InputKey.Crouch);
        }

        if (runToggle)
        {
            inputRun = input.GetKeyDown(InputManager.InputKey.Run);
            inputRoll = input.GetKeyDown(InputManager.InputKey.Roll);

            if (inputRun)
            {
                isRunning = !isRunning;
            }

            if (inputRoll && isCrouching)
            {
                isRolling = !isRolling;
                isRunning = !isRolling;
            }
        }
        else
        {

            inputRun = input.GetKey(InputManager.InputKey.Run);

            if (cc.velocity.magnitude > 0)
            {
                isRunning = inputRun;
            }

            if (isCrouching)
            {
                isRolling = input.GetKey(InputManager.InputKey.Roll);
                isRunning = false;
            }
        }

        // Check if you pressed the crouch input key and change the player's state


        // Run and Crouch animation
        // If dont have animator component, this block wont run
        if (animator != null)
        {
            if (cc.isGrounded)
            {
                animator.SetFloat("velocity", cc.velocity.magnitude);
                // Crouch
                // Note: The crouch animation does not shrink the character's collider
                animator.SetBool("crouch", isCrouching);

                animator.SetBool("roll", isRolling);
                // Run
                float minimumSpeed = 0.9f;
                animator.SetBool("walk", cc.velocity.magnitude > minimumSpeed && (inputHorizontal != 0 || inputVertical != 0));

                // Sprint
                // isRunning = cc.velocity.magnitude > minimumSpeed && inputRun;
                animator.SetBool("run", isRunning);
            }

            // Jump animation
            animator.SetBool("jump", cc.isGrounded == false);
        }

        // Handle can jump or not
        if ((input.GetKeyDown(InputManager.InputKey.Jump) || input.GetTouchDown() == "Jump") && cc.isGrounded)
        {
            isJumping = true;
            // Disable crounching when jumping
            //isCrouching = false; 
        }
    }

    private void GetHorizontal()
    {
        inputHorizontal = 0;

        if (input.GetKey(InputManager.InputKey.Left))
        {
            inputHorizontal = -1;
        }

        if (input.GetKey(InputManager.InputKey.Right))
        {
            inputHorizontal = 1;
        }

        if (input.GetAxis(InputManager.InputAxis.XaxisLeft))
        {
            inputHorizontal = input.GetAxisPosition(InputManager.InputAxis.XaxisLeft);
        }

        if (input.controllerConnected)
        {
            Debug.Log(input.GetAxisPosition(InputManager.InputAxis.XaxisLeft) + " left");
            Debug.Log(input.GetAxisPosition(InputManager.InputAxis.XaxisRight) + " right");
        }
    }

    private void GetVertical()
    {
        inputVertical = 0;

        if (input.GetKey(InputManager.InputKey.Back))
        {
            inputVertical = -1;
        }

        if (input.GetKey(InputManager.InputKey.Forward))
        {
            inputVertical = 1;
        }

        if (input.controllerConnected)
        {
            Debug.Log(input.GetAxisPosition(InputManager.InputAxis.YaxisBack) + " back");
            Debug.Log(input.GetAxisPosition(InputManager.InputAxis.YaxisForward) + " forward");
        }
    }
}
