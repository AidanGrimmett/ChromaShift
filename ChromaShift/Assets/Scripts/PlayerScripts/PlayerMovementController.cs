using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody worldRB;
    private Rigidbody rb;
    private CustomGravity gravityController;

    //Inputs
    private float xInput;
    private float zInput;
    private float jumpInput;
    public float mouseSensitivity;

    //movement
    [Header("Movement Params")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float inAirAccelerationMultiplyer;
    [SerializeField] private float inAirDecelerationMultiplyer;
    [SerializeField] private float CoyoteTime;
    private float targetSpeed;
    private float speedDif;
    private float accel;

    //jumping
    [Header("Jump Params")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallJumpForce;
    [SerializeField] private float jumpHangTimeThreshold;
    [SerializeField] private float jumpBufferTime;
    [SerializeField] private float maxFallSpeed;

    //Gravity
    [Header("Gravity Multipliers")]
    [SerializeField] private float defaultGravity;
    [SerializeField] private float jumpHangTimeGravity;
    [SerializeField] private float jumpFallGravity;
    [SerializeField] private float jumpCutGravity;
    [SerializeField] private float wallRunGravity;

    //Drag
    [Header("Drag")]
    [SerializeField] private float groundDrag;
    [SerializeField] private float airDrag;
    [SerializeField] private float wallDrag;

    //collisions
    [Header("Collisions")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector3 groundCheckSize;
    [SerializeField] private Vector3 wallCheckSize;
    [SerializeField] private Vector3 wallCheckOffset;

    private bool isJumping;
    private bool isJumpCut;
    private bool isJumpFalling;

    //Timers
    private float lastOnGround;
    private float lastOnWall;
    private float lastPressedJump;

    /* 
        Checklist!
        Movement
            - Running / strafe
            - building acceleration
        
        Jumping
            - Jump cuts
            

        Wall stuff
            - Wall running
            - Wall jumping

        Sliding
            - Lower character
            - Speed boost?
    */



    // Start is called before the first frame update
    void Start()
    {
        //Find objects
        worldRB = GameObject.Find("World").GetComponent<Rigidbody>();

        rb = GetComponent<Rigidbody>();
        gravityController = GetComponent<CustomGravity>();
    }

    // Update is called once per frame
    void Update()
    {
        //Timers
        lastOnGround -= Time.deltaTime;
        lastOnWall -= Time.deltaTime;
        lastPressedJump -= Time.deltaTime;

        //check terrain collisions
        if (Physics.OverlapBox(groundCheckPoint.position, groundCheckSize, Quaternion.identity, groundLayer).Length > 0)
        {
            lastOnGround = CoyoteTime;
            //Debug.Log("Touching ground");
        }
        if (Physics.OverlapBox(transform.position + wallCheckOffset, wallCheckSize, transform.rotation, groundLayer).Length > 0)
        {
            lastOnWall = CoyoteTime;
            Debug.Log("Touching wall");
        }

        //get inputs
        zInput = Input.GetAxisRaw("Vertical");
        xInput = Input.GetAxisRaw("Horizontal");

        //Jump inputs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInput();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            onJumpReleaseInput();
        }

        //jump rules

        //Jump triggers
        if (CanJump() && lastPressedJump > 0)
        {
            isJumping = true;
            isJumpCut = false;
            isJumpFalling = false;
            Jump();
        }
        else if (CanWallJump() && lastPressedJump > 0)
        {
            isJumping = true;
            isJumpCut = false;
            isJumpFalling = false;
            WallJump();
        }
        
        //peak of jump / starting to fall
        if (isJumping && rb.velocity.y < 0)
        {
            isJumping = false;
            isJumpFalling = true;
        }

        //On the ground / not jumping
        if (lastOnGround > 0 && !isJumping)
        {
            isJumpCut = false;
            isJumpFalling = false;
        }

        //gravity rules
        if ((isJumping || isJumpFalling) && Mathf.Abs(rb.velocity.y) < jumpHangTimeThreshold) //hangtime (light gravity)
        {
            SetGravityScale(jumpHangTimeGravity);
            Debug.Log("Hangning~");
        }
        else if (isJumpFalling)//falling from a jump (bit stronger)
        {
            SetGravityScale(jumpFallGravity);
            Debug.Log("Falling!");
        }
        else if (isJumpCut)//very strong to cancel out upwards momentum
        {
            SetGravityScale(jumpCutGravity);
            Debug.Log("Cutting!");
        }
        else if (lastOnWall > 0)
        {
            SetGravityScale(wallRunGravity);
            Debug.Log("Walling!");
        }
        else
        {
            SetGravityScale(defaultGravity); //normal gravity
        }

        //enforce max speeds
        //fall speed
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Max(rb.velocity.y), rb.velocity.z);

        //horizontal
        Vector3 flatVel = new Vector3(worldRB.velocity.x, 0, worldRB.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            worldRB.velocity = new Vector3(limitedVel.x, worldRB.velocity.y, limitedVel.z);
        }
    }

    private void Run()
    {
        //if (zInput != 0 || xInput != 0)
        //{
        //    Vector3 normalForward = transform.forward.normalized;

        //    Vector3 moveForce = normalForward * zInput + transform.right * xInput;

        //    worldRB.AddForce(-moveForce * moveSpeed);
        //}

        accel = 0;
        Vector3 flatVel = new Vector3(worldRB.velocity.x, 0, worldRB.velocity.z);

        targetSpeed = new Vector3(xInput, 0, zInput).magnitude * moveSpeed;
        if (lastOnGround > 0)
        {
            accel = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        }
        else
        {
            accel = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration * inAirAccelerationMultiplyer : deceleration * inAirDecelerationMultiplyer;
        }

        speedDif = (targetSpeed - flatVel.magnitude) * 2;
        Vector3 movement = (transform.right * xInput + transform.forward * zInput) * speedDif * accel;
        rb.AddForce(-movement);
    }

    private void Jump()
    {
        lastPressedJump = 0;
        lastOnGround = 0;

        float force = jumpForce;
        force += rb.velocity.y;

        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    private void WallJump()
    {

    }

    private void JumpCut()
    {

    }

    private void WallRun()
    {

    }
    private void SetGravityScale(float scale)
    {
        gravityController.SetGravityScale(scale);
    }

    private void FixedUpdate()
    {
        Run();
    }

    private void OnJumpInput()
    {
        lastPressedJump = jumpBufferTime;
    }

    private void onJumpReleaseInput()
    {
        if (CanCutJump()) isJumpCut = true;
    }

    private bool CanJump()
    {
        return (lastOnGround > 0 && !isJumping);
    }

    private bool CanCutJump()
    {
        return (isJumping && rb.velocity.y > 0);
    }

    private bool CanWallJump()
    {
        return (lastOnGround <= 0 && lastOnWall > 0 && lastPressedJump > 0 && !isJumping);
    }

    private void OnDestroy()
    {
        //ensure the mouse is freed when player is destroyed (exiting the game, going to menu etc)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
        Gizmos.DrawWireCube(transform.position + wallCheckOffset, wallCheckSize);
        Gizmos.DrawWireCube(transform.position - new Vector3(wallCheckOffset.x, Mathf.Abs(wallCheckOffset.y), wallCheckOffset.z), wallCheckSize);
    }
}
