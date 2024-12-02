using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    public float walkSpeed = 20;

    public float groundDrag;

    public  float playerHeight;
    public LayerMask whatIsGround;
    [SerializeField] bool grounded;

    public float sliding;
    public float crouchYScale;
    private float startYScale;

    public float jumpForce;
    public float jumpCooldownTime;
    public float airMultiplier;
    bool jumpCD;

    public KeyCode jumpkey = KeyCode.Space;
    public KeyCode slidekey = KeyCode.LeftControl;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sliding,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        if (grounded) { 
        rb.drag = groundDrag;
        Debug.Log("grounded"); }
        else
            rb.drag = 0;
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
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        //end crouch
        if (Input.GetKeyUp(slidekey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
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
        //movement calc
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10, ForceMode.Force);

        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10 * airMultiplier, ForceMode.Force);
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
}
