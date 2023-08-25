using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody worldRB;
    private Rigidbody rb;

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
    private float movement;

    //jumping
    [Header("Jump Params")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallJumpForce;
    [SerializeField] private float jumpHangTimeThreshold;
    [SerializeField] private float jumpBufferTime;
    [SerializeField] private float jumpHangTimeAccelMult;
    [SerializeField] private float jumpHangTimeSpeedMult;
    [SerializeField] private float maxFallSpeed;

    //Gravity
    [Header("Gravity Multipliers")]
    [SerializeField] private float defaultGravity;
    [SerializeField] private float jumpHangTimeGravity;
    [SerializeField] private float jumpFallGravity;
    [SerializeField] private float jumpCutGravity;

    //collisions
    [Header("Collisions")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector3 groundCheckSize;
    [SerializeField] private Vector3 wallCheckSize;
    [SerializeField] private Vector3 wallCheckOffset;

    public bool isJumping { get; private set; }
    private bool isJumpCut;
    public bool isJumpFalling { get; private set; }

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

        //set up the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseSensitivity *= 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        //Timers
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
        float mouseX = Input.GetAxis("Mouse X");

        zInput = Input.GetAxisRaw("Vertical");
        xInput = Input.GetAxisRaw("Horizontal");
        float jumpInput = Input.GetAxisRaw("Jump");

        //rotate character with mouse movement
        transform.Rotate(new Vector3(0f, mouseX * mouseSensitivity, 0f));
       
        if (jumpInput > 0)
        {
            rb.AddForce(0, 0.2f, 0, ForceMode.Impulse);
        }
    }

    private void UpdateInputs()
    {

    }

    private void Run()
    {

    }

    private void Jump()
    {

    }

    private void JumpCut()
    {

    }

    private void WallRun()
    {

    }

    private void FixedUpdate()
    {
        if (zInput != 0 || xInput != 0)
        {
            Vector3 normalForward = transform.forward.normalized;

            Vector3 moveForce = normalForward * zInput + transform.right * xInput;

            worldRB.AddForce(-moveForce * moveSpeed);
        }
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
