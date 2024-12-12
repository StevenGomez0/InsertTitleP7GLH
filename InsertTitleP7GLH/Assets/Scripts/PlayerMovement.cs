using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public UnityEvent OnDeath;

    private float moveSpeed;
    public float walkSpeed = 20;

    private float startingHealth;
    public float Health;

    public Gun1 Gun1;

    private float startGroundDrag;
    public float groundDrag;
    public float slidingDrag;
    public float airDrag;

    public  float playerHeight;
    public LayerMask whatIsGround;
    [SerializeField] bool grounded;
    [SerializeField] bool sliding;

    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    public float crouchYScale;
    private float startYScale;

    public float jumpForce;
    public float jumpCooldownTime;
    public float airMultiplier;
    bool jumpCD;

    public KeyCode jumpkey = KeyCode.Space;
    public KeyCode slidekey = KeyCode.LeftControl;

    public Transform orientation;

    public float horizontalInput;
    public float verticalInput;

    Vector3 moveDirection;

    public Rigidbody rb;

    public float tempDampTime;
    private float tempx;
    private float tempz;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sliding,
        air
    }

    public enum GunState
    {
        gun1,
        gun2
    }

    private void Start()
    {
        startingHealth = Health;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;
        startGroundDrag = groundDrag;
    }

    private void Update()
    {
       // Debug.Log(rb.velocity.normalized);
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        

        MyInput();
        SpeedControl();
        StateHandler();

        if (grounded) { rb.drag = groundDrag; 
        //Debug.Log("grounded");
        }
        else
            rb.drag = airDrag;

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && grounded && !sliding)
        {
            //so this should quickly stop movement if there is no input in any direction, im doing this intstead of adjusting drag to not mess with the movespeed or something
            //
            //rb.velocity = Vector3.zero;
            rb.velocity = new Vector3(Mathf.SmoothDamp(rb.velocity.x, 0, ref tempx, tempDampTime), 0, Mathf.SmoothDamp(rb.velocity.z, 0, ref tempz, tempDampTime));
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //jump
        if(Input.GetKey(jumpkey) && !jumpCD && grounded)
        {
            jumpCD = true;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldownTime);
        }

        //start crouch
        if(Input.GetKey(slidekey) && grounded)
        {
            sliding = true;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            groundDrag = slidingDrag;
            rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
            if (Input.GetKeyDown(slidekey))
            {
                rb.AddForce(orientation.forward * moveSpeed * 3.0f, ForceMode.Impulse);
            }
        }

        //end crouch
        if (Input.GetKeyUp(slidekey))
        {
            sliding = false;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            groundDrag = startGroundDrag;
        }
    }

    private void StateHandler()
    {
        //walking
        if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        if (Input.GetKeyDown(slidekey) && grounded)
        {
            state = MovementState.sliding;
            //sliding movement equation
        }
        //air
        else
        {
            state = MovementState.air;
        }
    }

    
    private void MovePlayer()
    {
        if (!sliding)
        {
            //movement calc
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (OnSlope())
            {
                rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 5f, ForceMode.Force);
            }


            if (grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10, ForceMode.Force);

            else if (!grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10 * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude >moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        jumpCD = false;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    public void Damaged(float damage, Transform enemy)
    {
        Debug.Log("IS EXPLODING");
        Health -= damage;
        rb.AddExplosionForce(damage, enemy.position, 10, 0);
        if (Health <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}
