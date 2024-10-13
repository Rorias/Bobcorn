using System;
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
    public float sprintAdittion = 3.5f;
    [Tooltip("The higher the value, the higher the character will jump.")]
    public float jumpForce = 18f;
    [Tooltip("Stay in the air. The higher the value, the longer the character floats before falling.")]
    public float jumpTime = 0.85f;
    [Space]
    [Tooltip("Force that pulls the player down. Changing this value causes all movement, jumping and falling to be changed as well.")]
    public float gravity = 9.8f;


    private float jumpElapsedTime = 0;

    // Player states
    private bool isJumping = false;
    private bool isSprinting = false;
    private bool isCrouching = false;

    // Inputs
    private float inputHorizontal;
    private float inputVertical;
    private bool inputJump;
    private bool inputCrouch;
    private bool inputSprint;


    public float movementSpeedMultiplier;

    private InputManager input;

    private List<RaycastResult> rayResults = new List<RaycastResult>();

    private Animator animator;
    private CharacterController cc;

    private Vector2 startPos;
    private Vector2 touchPos;

    private bool pressed;

    private void Awake()
    {
        input = InputManager.Instance;

        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
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
            PcMovement();
        }
    }

    private void FixedUpdate()
    {
        float velocityAdittion = 0;

        if (isSprinting)
        {
            velocityAdittion = sprintAdittion;
        }

        if (isCrouching)
        {
            velocityAdittion = -(velocity * 0.50f); // -50% velocity
        }

        // Direction movement
        float directionX = inputHorizontal * (velocity + velocityAdittion) * Time.deltaTime;
        float directionZ = inputVertical * (velocity + velocityAdittion) * Time.deltaTime;
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
        //movePad.gameObject.SetActive(false);
        // movePadSlider.gameObject.SetActive(false);
    }

    private void InitMoveCamera()
    {
        pressed = true;
        //initialField = TouchField.camera;
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

    private void MoveCamera()
    {

    }

    private void MoveInDirection()
    {
        //Vector2 moveOffset = Vector2.ClampMagnitude(touchPos - startPos, movePad.rect.width / 2);
        //movePadSlider.anchoredPosition = initialMovePadPos;
        //movePadSlider.anchoredPosition = initialMovePadPos + moveOffset;
        //Vector2 moveSpeed = new Vector2(moveOffset.x / (movePad.rect.width / 2), moveOffset.y / (movePad.rect.width / 2));
        //cc.SimpleMove(new Vector3(moveSpeed.x, 0, moveSpeed.y) * movementSpeedMultiplier);
    }

    private void PcMovement()
    {
        inputHorizontal = input.GetKey(InputManager.InputKey.Left) ? -1 : 0;
        inputHorizontal = input.GetKey(InputManager.InputKey.Right) ? 1 : 0;
        Debug.Log(input.GetAxisPosition(InputManager.InputAxis.XaxisLeft) + " left");
        Debug.Log(input.GetAxisPosition(InputManager.InputAxis.XaxisRight) + " right");
        inputVertical = Input.GetAxis("Vertical");

        inputSprint = input.GetKey(InputManager.InputKey.Run);

        inputCrouch = input.GetKeyDown(InputManager.InputKey.Crouch);

        // Check if you pressed the crouch input key and change the player's state
        if (inputCrouch)
        {
            isCrouching = !isCrouching;
        }

        // Run and Crouch animation
        // If dont have animator component, this block wont run
        if (cc.isGrounded && animator != null)
        {
            // Crouch
            // Note: The crouch animation does not shrink the character's collider
            animator.SetBool("crouch", isCrouching);

            // Run
            float minimumSpeed = 0.9f;
            animator.SetBool("run", cc.velocity.magnitude > minimumSpeed);

            // Sprint
            isSprinting = cc.velocity.magnitude > minimumSpeed && inputSprint;
            animator.SetBool("sprint", isSprinting);

        }

        // Jump animation
        if (animator != null)
        {
            animator.SetBool("air", cc.isGrounded == false);
        }

        // Handle can jump or not
        if ((input.GetKeyDown(InputManager.InputKey.Jump) || input.GetTouchDown() == "Jump") && cc.isGrounded)
        {
            isJumping = true;
            // Disable crounching when jumping
            //isCrouching = false; 
        }
    }
}
