using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementTutorial : MonoBehaviour
{
    [Header("Movement")]
    public bool allowMovement;
                    internal float moveSpeed;
    [SerializeField] float slideSpeed;
    [SerializeField] float swingSpeed;
    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;
           float desiredMoveSpeed;
           float lastDesiredMoveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    [SerializeField] float wallRunSpeed;

    bool readyToJump;

     public float walkSpeed;
    public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.C;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Crouching")]
    [SerializeField] float crouchSpeed;
    [SerializeField] float crouchYScale;
    float startYScale;

    [Header("Slope")]
    [SerializeField] float maxSlopeAngle;
    RaycastHit slopeHit;
    bool exitingSlope;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    [SerializeField] internal float startingPositionZ;
    float distanceCovered;

    Rigidbody rb;

    public bool sliding;
    public bool wallrunning;
    public bool swinging;
    public MovementState movementState;
    public enum MovementState
    {
        walking,
        sprinting,
        air,
        crouching,
        wallrunning,
        sliding,
        swinging
    }
    private void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;
        readyToJump = true;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        if (GameManager.Instance.startGame)
        {
            distanceCovered = (Time.time * moveSpeed) + startingPositionZ;

            // Display the distance covered in the console
          //  Debug.Log("Distance Covered: " + distanceCovered);
            MyInput();
            SpeedControl();
            StateHandle();
        }
        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        //Debug.Log(moveSpeed);
    }

    private void FixedUpdate()
    {

        
            if (allowMovement)
            {
                MovePlayer();
            }
            else
            {
                AutoMovePlayer();
            }
        
    }

    void StateHandle()
    {

        if (wallrunning)
        {
            movementState = MovementState.wallrunning;

            desiredMoveSpeed = wallRunSpeed;
        }
        if (sliding)
        {
            movementState = MovementState.sliding;
      
                desiredMoveSpeed = slideSpeed;    
        }

        else if (swinging) {
            movementState = MovementState.swinging;

            desiredMoveSpeed = swingSpeed;
        }
        else if (Input.GetKey(crouchKey))
        {
            movementState = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        else if(grounded && Input.GetKey(sprintKey)) 
        {
            movementState = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        else if(grounded)
        {
            movementState = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        else
        {
            movementState = MovementState.air;

        }

        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed!=0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }
        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private void MyInput()
    {
       
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x,crouchYScale,transform.localScale.z);
            rb.AddForce(Vector3.down * 5 , ForceMode.Impulse);
        }

        //stop crouch
        if(Input.GetKeyUp(crouchKey)) 
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    IEnumerator SmoothLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }
    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        // on ground
        else if (grounded)
        {
           
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            
        }
        // in air
        else if (!grounded)
        {  
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
          
        }

    }

    private void AutoMovePlayer()
    {
        // calculate movement direction
        //moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(transform.forward) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        // on ground
        else if (grounded)
        {   
                rb.AddForce(transform.forward * moveSpeed * 10f, ForceMode.Force);
                  
        }
        // in air
        else if (!grounded)
        {
           
                rb.AddForce(transform.forward * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            
        }

    }
    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }

        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;   
    }

    public bool OnSlope()
    {

        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit , playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction,slopeHit.normal).normalized;
    }
}