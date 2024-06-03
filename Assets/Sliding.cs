using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sliding : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Transform orientation;
    Rigidbody rb;
    PlayerMovementTutorial pm;
    [SerializeField] LayerMask slideLayer;
    [Header("Sliding")]
    [SerializeField] float maxSlideTime;
    [SerializeField] float slideForce;
    float slideTimer;

    [SerializeField] float slideYScale;
    float startYScale;


    public MouseButton slide = MouseButton.Right;
    float horizontalInput;
    float verticalInput;

   

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementTutorial>();
       
        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (GameManager.Instance.startGame)
        {
            if (Physics.Raycast(transform.position, Vector3.down, 2 * 3f + 0.3f, slideLayer))
            {
                if (pm.allowMovement)
                {
                    if (Input.GetMouseButtonDown(1) && (horizontalInput != 0 || verticalInput != 0))
                    {
                        StartSlide();
                    }

                    if (Input.GetMouseButtonUp(1) && pm.sliding)
                    {
                        StopSlide();
                    }
                }

                else
                {

                    if (Input.GetMouseButtonDown(1))
                    {
                        StartSlide();
                    }

                    if (Input.GetMouseButtonUp(1) && pm.sliding)
                    {
                        StopSlide();
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {


        if (pm.sliding)
        {
            if (pm.allowMovement)
            {

                SlidingMovement();
            }
            else
            {
                SlidingMovementAuto();
            }
        }
    }
    void StartSlide()
    {
        pm.sliding = true;

        transform.localScale = new Vector3(transform.localScale.x, slideYScale, transform.localScale.z);

        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        if(slideTimer <= 0 )
        {   
            
            StopSlide() ;
        }
    }

    void SlidingMovementAuto()
    {
       

        if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(transform.forward.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(transform.forward) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0)
        {

            StopSlide();
        }
    }

    void StopSlide()
    {
        pm.sliding =false;

        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
    }
}
