using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public UnityEvent OnDeath;
    private bool isDead;
    public UnityEvent OnHit;

    public Image healthBar;

    private bool slideBoostCD;
    public float slideBoostCDValue;

    public float InvincibilityLength;
    private bool isInvincible;

    private float moveSpeed;
    public float walkSpeed = 20;
    public float gravityMultiplier;

    public float startingHealth; //is there a way to make a variable not visible on inspector but accessable by other scripts? that would be very nice here (thats not static)
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

    /*  unused floats    public float tempDampTime;
    private float tempx;
    private float tempz;*/

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
    }

    private void FixedUpdate()
    {
        if(!isDead) MovePlayer();
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
            if (Input.GetKeyDown(slidekey) && !slideBoostCD)
            {
                rb.AddForce(orientation.forward * moveSpeed * 3.0f, ForceMode.Impulse);
            }
            Debug.Log("slide boost cooldown on");
            if (!slideBoostCD) Invoke(("slideBoostTimer"), slideBoostCDValue);
            slideBoostCD = true;
        }

        //end crouch
        if (Input.GetKeyUp(slidekey))
        {
            sliding = false;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            groundDrag = startGroundDrag;
        }

        if (transform.position.x > 70 || transform.position.x < -70 || transform.position.z > 70 || transform.position.z < -70 || transform.position.y < -10 || transform.position.y > 50)
        {
            if(Health > 0)
            {
                Debug.Log("out of bounds, killed");
                Damaged(Health);
            }
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
        if(!grounded) rb.AddForce(Vector3.down * 600 * gravityMultiplier);
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

    public void Damaged(float damage)
    {
        Debug.Log("IS EXPLODING");
        if (!isInvincible)
        {
            Health -= damage;
            OnHit?.Invoke();
            isInvincible = true;
            Invoke("boolTimer", InvincibilityLength);
        }
        else 
        { 
            Debug.Log("invincible, null damage"); 
        }
        if (Health <= 0)
        {
            isDead = true;
            OnDeath?.Invoke();
        }
    }

    void boolTimer()
    {
        isInvincible = false;
    }

    void slideBoostTimer()
    {
        //i know this is a really redundant method because theres a perfectly functional method right above it but i cant think critically rn (11:30 pm)
        Debug.Log("slide cd off");
        slideBoostCD = false;
    }
}
