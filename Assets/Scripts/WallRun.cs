using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WallRun : MonoBehaviour
{
    [Header("WallRun")]
    [SerializeField] LayerMask whatIsWall;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float wallRunForce;
    [SerializeField] float maxWallRunTime;
    [SerializeField] float wallJumpUpForce;
    [SerializeField] float wallJumpSideForce;
    float wallRunTimer;

    [Header("ExitingWall")]
    bool exitingWall;
    [SerializeField] float exitWallTime;
    float exitWallTimer;
    
    float horizontalInput, verticalInput;

    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Detection")]
    [SerializeField] float wallCheckDistance;
    [SerializeField] float minJumpHeight;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    bool wallLeft, wallRight;

    [Header("Reference")]
    [SerializeField] Transform orientation;
    Rigidbody rb;
    PlayerMovementTutorial pm;
    [SerializeField] PlayerCam camScript;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementTutorial>();
    }

    private void Update()
    {
        CheckWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
        {
            WallRunMovement();
        }
    }
    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
        wallRight = Physics.Raycast(transform.position, orientation.right, out  rightWallHit, wallCheckDistance,whatIsWall);
    }

    bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    void StateMachine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        if((wallLeft || wallRight) && verticalInput >0 && AboveGround() && !exitingWall)
        {
            if(!pm.wallrunning)
            {
                StartWallRun();
            }

            if (Input.GetKeyDown(jumpKey))
            {
                WallJump();
            }
        }
        else if (exitingWall)
        {
            if (pm.wallrunning)
            {
                StopWallRun();

            }

            if(exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }

            if(exitWallTimer < 0)
            {
                exitingWall = false;
            }
        }
        else
        {
            if(pm.wallrunning)
            {
                StopWallRun();
            }
        }
    }

    void StartWallRun()
    {
        pm.wallrunning = true;

        camScript.DoFOV(90f);
        if(wallLeft)
        {
            camScript.DoTilt(-5f);
        }
        if (wallRight)
        {
            camScript.DoTilt(5f);
        }
    }

    void WallRunMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x , 0 , rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);
        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        rb.AddForce(wallForward * wallRunForce , ForceMode.Force);

        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
       

    }

    void StopWallRun()
    {
        pm.wallrunning = false;
        camScript.DoFOV(80f);
        camScript.DoTilt(0f);

        rb.useGravity = true;
    }

    void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x , 0f, rb.velocity.z);
        rb.AddForce(forceToApply , ForceMode.Impulse);
    }
}
