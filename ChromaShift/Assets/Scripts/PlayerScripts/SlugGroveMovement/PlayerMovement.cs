using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PlayerMovement : MonoBehaviour
{
    public enum PlayerStates
    {
        Grounded,//on the ground
        InAir, //in the air
        OnWalls, //running on the walls
    }

    private PlayerCollision Coli;
    private Rigidbody Rigid;
    private Rigidbody PlayerRB;
    private CapsuleCollider Cap;
    private CustomGravity gravityController;

    private float XMove;
    private float YMove;

    [Header("Physics")]
    public float maxSpeed; //how fast we run forward
    public float speedCap;
    public float backwardsSpeed; //how fast we run backwards
    public float inAirControl; //how much control you have over your movement direction when in air

    private float ActSpeed; //how much speed is applied to the rigidbody
    public float Acceleration; //how fast we build speed
    public float Decceleration; //how fast we slow down
    public float DirectionControl = 8; //how much control we have over changing direction
    public PlayerStates CurrentState; //the current state the player is in
    private float InAirTimer; //how long we are in the air for (this is for use when wall running or falling off the ground
    private float OnGroundTimer;
    private float AdjustmentAmt; //the amount added to our player acceleration, this is used for adjusting to new speeds such as when we slide

    [Header("Gravity")]
    public float defaultGravity = 1;
    public float wallGravity = 0.5f;
    public float fallGravity = 1.5f;
    public float maxFallSpeed = 10f;


    [Header("Turning")]
    public float TurnSpeed; //how fast we turn when on the ground
    public float TurnSpeedInAir; //how fast we turn when in air
    public float TurnSpeedOnWalls; //how fast we turn when on the walls
    public float LookUpSpeed; //how fast we look up and down
    public Camera Head; //what will function as our players head to tilt up and down (this is a pivot point in our model that the cameras are children of
    private float YTurn; //how much we have turned left and right
    private float XTurn; //how much we have turned Up or Down
    public float MaxLookAngle = 65; //how much we can look up
    public float MinLookAngle = -30; //how much we can look down

    [Header("Jumping")]
    public float JumpHeight; //how high we jump
    public float WallJumpVerticalHeight; //jump power in the y axis
    public float WallJumpHorizontalStrength; //jump power in the x axis
    public float WallJumpForwardBoost;
    public float coyoteTime = 0.15f;

    [Header("Wall Runs")]
    public float WallRunTime = 2f; //how long we can run on walls
    private float ActWallRunTime = 0; //how long we are actually on a wall
    public float TimeBeforeWallRun = 0.2f; //how long we have to be in the air before we can wallrun
    public float WallRunUpwardsMovement = 2f; //how much we move up a wall when running on it (make this 0 to just slightly move down a wall we run on
    public float WallRunSpeedAcceleration = 2f; //how quickly we build speed to run up walls
    public float cameraTiltAmount = 15; //how much the camera tilts when on a wall
    public float tiltLerpTime = 0.15f; //how fast the transition is
    private float currentTiltAngle = 0;
    public float wallRunLerpTimeToFlat;
    public float wallRunSpeed = 13;
    public float lerpTimeToSpeed = 0.05f;

    [Header("Crouching")]
    public float CrouchSpeed = 10; //how fast we move when crouching
    public float CrouchHeight = 1.5f; //how tall our capsule will be when crouched
    private float StandingHeight = 2f; //this is how tall our capsule is
    private bool Crouch;

    [Header("Sliding")]
    public float SlideAmt; //how far we slide when pressing crouch
    public float SlideSpeedLimit; //how fast we have to be traveling before a crouch will trigger a slide
    public float SlideControl; //how much we adjust to our slide speed and regain player control
    public float SlideTimer = 0.4f;
    private float lastSlide = 0;

    [Header("FOV")]
    public float MaxFov;
    private float MinFov;
    public float FOVSpeed; //how fast we must go before we reach max fov'


    //Debug things
    private Vector3 savePosPlayer = Vector3.zero;
    private Vector3 savePosWorld = Vector3.zero;
    public bool jumpAtLook;


    // Start is called before the first frame update
    void Start()
    {
        Coli = GetComponent<PlayerCollision>();
        Rigid = GameObject.Find("World").GetComponent<Rigidbody>();
        PlayerRB = GetComponent<Rigidbody>();
        MinFov = Head.fieldOfView;
        Cap = GetComponent<CapsuleCollider>();
        StandingHeight = Cap.height;
        gravityController = GetComponent<CustomGravity>();

        AdjustmentAmt = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SavePos();
    }

    // Update is called once per frame
    void Update()
    {
        XMove = Input.GetAxisRaw("Horizontal");
        YMove = Input.GetAxisRaw("Vertical");


        DebugControls();

        //tilt head
        Transform camTrans = Head.transform;
        float targetAngle = 0;
        if (CurrentState == PlayerStates.OnWalls)
            targetAngle = Coli.CheckLeftWall() ? -cameraTiltAmount : Coli.CheckRightWall() ? cameraTiltAmount : 0f;

        if (Mathf.Approximately(ActSpeed, 0)) ActSpeed = 0;

        if (ActSpeed > speedCap)
        {
            ActSpeed = speedCap;
        }

        currentTiltAngle = Mathf.Lerp(currentTiltAngle, targetAngle, Time.deltaTime * tiltLerpTime);

        if (CurrentState == PlayerStates.Grounded)
        {
            if (new Vector2(XMove, YMove).magnitude == 0) //Rigid.velocity.magnitude <= Mathf.Abs(1f) && 
            {
                gravityController.SetGravityScale(0);
                PlayerRB.velocity = Vector3.zero;
            }
            else
            {
                gravityController.SetGravityScale(defaultGravity);
            }

            //if we press jump
            if (Input.GetButtonDown("Jump"))
            {
                //jump upwards
                JumpUp();
            }
        }
        else if (CurrentState == PlayerStates.InAir)
        {
            //if we press jump
            if (Input.GetButtonDown("Jump") && InAirTimer < coyoteTime)
            {
                //jump upwards
                JumpUp();
            }

            //Check if there is a wall to run on
            bool Wall = CheckWalls(XMove, YMove);

            //we are on the wall
            if (Wall)
            {
                if (InAirTimer > TimeBeforeWallRun)
                {
                    SetOnWall();
                    return;
                }
            }

            //check for the ground 
            bool Grounded = Coli.CheckFloor(-transform.up);

            //we are on the ground (and have been in the air for a short time, to prevent multiple jump glitched
            if (Grounded && InAirTimer > 0.25f)
            {
                SetOnGround();
            }

            if (PlayerRB.velocity.y < -maxFallSpeed)
            {
                PlayerRB.velocity = new Vector3(PlayerRB.velocity.x, -maxFallSpeed, PlayerRB.velocity.z);
            }
        }
        else if (CurrentState == PlayerStates.OnWalls)
        {
            //if we press jump
            if (Input.GetButtonDown("Jump"))
            {
                //lastWalls = Coli.GetWallNames();
                //jump upwards
                JumpUp();
            }

            //Check if there is a wall to run on
            bool Wall = CheckWalls(XMove, YMove);

            //we are no longer on the wall, fall off it
            if (!Wall)
            {
                SetInAir();
                return;
            }

            //check for the ground 
            bool Grounded = Coli.CheckFloor(-transform.up);

            //we are on the ground
            if (Grounded)
            {
                SetOnGround();
            }
        }
    }

    private void FixedUpdate()
    {
        float Del = Time.deltaTime;

        //get our players rotation amount for turning
        float CamX = Input.GetAxisRaw("Mouse X");
        float CamY = Input.GetAxisRaw("Mouse Y");

        //have our player look up and down
        LookUpDown(CamY, Del);

        //handle our fov
        HandleFov(Del);

        //get inputs
        float horInput = Input.GetAxisRaw("Horizontal");
        float verInput = Input.GetAxisRaw("Vertical");

        if (CurrentState == PlayerStates.Grounded)
        {
            //tick our ground timer
            if (OnGroundTimer < 10)
                OnGroundTimer += Del;


            //get magnituded of our inputs
            float InputMagnitude = new Vector2(horInput, verInput).normalized.magnitude;
            //get the amount of speed, based on if we press forwards or backwards
            float TargetSpd = Mathf.Lerp(backwardsSpeed, maxSpeed, verInput); //using the vertical input as a lerp from if forward is being pressed
            //if we are crouching our target speed is our crouch speed
            if (Crouch && !(lastSlide - Time.time < -SlideTimer))
                TargetSpd = CrouchSpeed;

            if (verInput > 0 && Rigid.velocity.magnitude > TargetSpd)
            {
                //LerpSpeed(InputMagnitude, Del, Rigid.velocity.magnitude);
            }
            else
            {
                LerpSpeed(InputMagnitude, Del, TargetSpd);
            }

            MovePlayer(horInput, verInput, Del);
            TurnPlayer(CamX, Del, TurnSpeed);

            //check for crouching 
            if (Input.GetButton("Crouching"))
            {
                //start crouching
                if (!Crouch)
                {
                    StartCrouch();
                }
            }
            else
            {
                //stand up
                bool check = Coli.CheckRoof(transform.up);
                if (!check)
                {
                    StopCrouching();
                }
            }

            //add to our player adjustment
            if (AdjustmentAmt < 1)
                AdjustmentAmt += Del * SlideControl;
            else
                AdjustmentAmt = 1;

            //check for the ground 
            bool Grounded = Coli.CheckFloor(-transform.up);

            //we are in the air
            if (!Grounded)
            {
                if (InAirTimer < 0.2f)
                    InAirTimer += Del;
                else
                {
                    SetInAir();
                    return;
                }
            }
            else
            {
                //we are on the ground to remove any increase in the air timer
                InAirTimer = 0;
                SetGravity(defaultGravity);
            }
        }
        else if (CurrentState == PlayerStates.InAir)
        {
            //Debug.Log("In Air!!");
            //tick our Air timer
            if (InAirTimer < 10)
                InAirTimer += Del;

            MoveInAir(horInput, verInput, Del);

            AdjustmentAmt = 1;

            //turn our player with the in air modifier
            TurnPlayer(CamX, Del, TurnSpeedInAir);
            if (PlayerRB.velocity.y < 0)
            {
                SetGravity(fallGravity);
            }
            else
            {
                SetGravity(defaultGravity);
            }
        }
        else if (CurrentState == PlayerStates.OnWalls)
        {
            //tick our wall run timer
            ActWallRunTime += Del;
            //Debug.Log("On Wall!");
            //turn our player with the in air modifier
            TurnPlayer(CamX, Del, TurnSpeedOnWalls);

            SetGravity(0);
            Vector3 LerpVelocity = Vector3.Lerp(PlayerRB.velocity, Vector3.zero, wallRunLerpTimeToFlat);
            if (Coli.CheckLeftWall() || Coli.CheckRightWall())
            {
                Vector3 normalizedVelocity = Rigid.velocity.normalized;

                // Check if the velocity direction is opposite to the transform.forward
                if (Vector3.Dot(normalizedVelocity, transform.forward) > 0)
                {
                    // Reverse the direction of the velocity
                    Rigid.velocity = -Rigid.velocity;
                }

                Vector3 LerpWorldVelocity = Vector3.Lerp(Rigid.velocity, Rigid.velocity.normalized * wallRunSpeed, lerpTimeToSpeed);
                Rigid.velocity = LerpWorldVelocity;
            }
            PlayerRB.velocity = LerpVelocity;
        }
    }

    //lerp our current speed to our set max speed, by how much we are pressing the horizontal and vertical input
    void LerpSpeed(float InputMag, float D, float TargetSpeed)
    {
        //multiply our speed by our input amount
        float LerpAmt = TargetSpeed * InputMag;
        //get our acceleration (if we should speed up or slow down
        float Accel = Acceleration;
        if (InputMag == 0)
            Accel = Decceleration;
        //lerp by a factor of our acceleration
        ActSpeed = Mathf.Lerp(ActSpeed, LerpAmt, D * Accel);
    }

    //when in the air or on a wall, we set our action speed to the velocity magnitude, this is so that when we reach the ground again, our speed will carry over our momentum
    void SetSpeedToVelocity()
    {
        float Mag = new Vector2(Rigid.velocity.x, Rigid.velocity.z).magnitude;
        ActSpeed = Mag;
    }

    bool CheckWalls(float X, float Y)
    {
        if (X == 0 && Y == 0) //if no direction input we are not wall running
            return false;

        if (ActWallRunTime >= WallRunTime) //if our wall run timer is more than the amount we can run on walls for, we cannot wall run
            return false;

        //check the collision direction for any walls
        Vector3 Dir = transform.right * X;

        bool WallCol = Coli.CheckWall(Dir);

        return WallCol;
    }

    void SetInAir()
    {
        StopCrouching(); //cannot crouch in air

        OnGroundTimer = 0; //remove the on ground timer
        CurrentState = PlayerStates.InAir;
    }

    void SetOnGround()
    {
        //set our current speed to our velocity
        SetSpeedToVelocity();

        ActWallRunTime = 0; //we are on the ground again, our wall run timer is reset
        InAirTimer = 0; //remove the in air timer
        CurrentState = PlayerStates.Grounded;
    }

    void SetOnWall()
    {
        CurrentState = PlayerStates.OnWalls;
    }

    void PrintStrings(string[] strs)
    {
        string output = "";
        foreach (string s in strs)
        {
            output += s + "\n";
        }

        Debug.Log(output);
    }

    void StartCrouch()
    {
        Crouch = true;
        Cap.height = CrouchHeight;

        if (ActSpeed > SlideSpeedLimit)
            SlideSelf();
    }

    void StopCrouching()
    {
        Crouch = false;
        Cap.height = StandingHeight;
    }

    void TurnPlayer(float Hor, float D, float turn)
    {
        //add our inputs to our turn value
        YTurn += (Hor * D) * turn;
        //turn our character
        transform.rotation = Quaternion.Euler(0, YTurn, 0);
    }

    void LookUpDown(float Ver, float D)
    {
        //add our inputs to our look angle
        XTurn -= (Ver * D) * LookUpSpeed;
        XTurn = Mathf.Clamp(XTurn, MinLookAngle, MaxLookAngle);
        //look up and down
        Head.transform.localRotation = Quaternion.Euler(XTurn, 0, currentTiltAngle);
    }

    void MovePlayer(float Hor, float Ver, float D)
    {
        //find the direction to move in, based on the direction inputs
        Vector3 MovementDirection = (transform.forward * Ver) + (transform.right * Hor);
        MovementDirection = MovementDirection.normalized;
        //if we are no longer pressing and input, carryon moving in the last direction we were set to move in
        if (Hor == 0 && Ver == 0)
            MovementDirection = Rigid.velocity.normalized;

        MovementDirection = MovementDirection * ActSpeed;

        //apply Gravity and Y velocity to the movement direction 
        MovementDirection.y = Rigid.velocity.y;

        //apply adjustment to acceleration
        float Acel = DirectionControl * AdjustmentAmt;
        Vector3 LerpVelocity = Vector3.Lerp(Rigid.velocity, -MovementDirection, Acel * D);
        Rigid.velocity = LerpVelocity;
    }

    void MoveInAir(float Hor, float Ver, float D)
    {
        //find the direction to move in, based on the direction inputs
        Vector3 MovementDirection = (transform.forward * Ver) + (transform.right * Hor);
        MovementDirection = MovementDirection.normalized;
        //if we are no longer pressing and input, carryon moving in the last direction we were set to move in
        if (Hor == 0 && Ver == 0)
            MovementDirection = Rigid.velocity.normalized;

        MovementDirection = MovementDirection * ActSpeed;

        Vector2 strafeInputs = new Vector2(Hor, 0).normalized;

        //apply Gravity and Y velocity to the movement direction 
        MovementDirection.y = Rigid.velocity.y;

        //lerp to our movement direction based on how much airal control we have
        Vector3 LerpVelocity = Vector3.Lerp(Rigid.velocity, -MovementDirection, strafeInputs.magnitude > 0.1f ? inAirControl : 0.5f * D);
        Rigid.velocity = LerpVelocity;
    }

    void WallMove(float D)
    {
        //get the direction to run up this wall if we press forward (keep in mind this only works if the wall is infront or to the side of the player as we run along on, on walls to our immediate right or left we slide down
        Vector3 MovementDirection = transform.up;
        MovementDirection = MovementDirection * WallRunUpwardsMovement;

        //our x z velocity are our momentum applied to our forward direction
        MovementDirection += transform.forward * ActSpeed;

        Vector3 LerpVelocity = Vector3.Lerp(Rigid.velocity, MovementDirection, WallRunSpeedAcceleration * D);
        Debug.Log(LerpVelocity);
        PlayerRB.velocity = LerpVelocity;
    }

    void JumpUp()
    {
        //only jump if we are still on the ground
        if (CurrentState == PlayerStates.Grounded)
        {
            //reduce our velocity on the y axis so our jump force can be added
            Vector3 VelAmt = PlayerRB.velocity;
            VelAmt.y = 0;
            PlayerRB.velocity = VelAmt;
            //add our jump force
            PlayerRB.AddForce(transform.up * JumpHeight, ForceMode.Impulse);
            //we are now in the air
            SetInAir();
        }
        else if (CurrentState == PlayerStates.OnWalls || (CurrentState == PlayerStates.InAir && InAirTimer < coyoteTime))
        {
            //reduce our velocity on the y axis so our jump force can be added
            Vector3 VelAmt = PlayerRB.velocity;
            //Vector3 flatVelNorm = new Vector3(PlayerRB.velocity.x, 0, PlayerRB.velocity.z).normalized;
            VelAmt.y = 0;
            PlayerRB.velocity = VelAmt;
            //add our jump force
            Vector3 forceToAdd = transform.up * WallJumpVerticalHeight;

            //extra height if looking up
            Transform camTrans = Head.transform;
            Vector3 lookDir = camTrans.forward.normalized;
            forceToAdd += Vector3.up * 3 * (1 * Mathf.Clamp(lookDir.y, 0f, 1f));
            if (Coli.CheckLeftWall())
            {
                forceToAdd += -transform.right * WallJumpHorizontalStrength;
            }
            else if (Coli.CheckRightWall())
            {
                forceToAdd += transform.right * WallJumpHorizontalStrength;
            }
            forceToAdd += -transform.forward * YMove * WallJumpForwardBoost;
            PlayerRB.AddForce(forceToAdd, ForceMode.Impulse);
            //we are now in the air
            SetInAir();
        }
    }

    //increase our fov at high speed and reduce it at low speed
    void HandleFov(float D)
    {
        //get our velocity magniture
        float mag = new Vector2(Rigid.velocity.x, Rigid.velocity.z).magnitude;
        //get appropritate fov 
        float LerpAmt = mag / FOVSpeed;
        float FieldView = Mathf.Lerp(MinFov, MaxFov, LerpAmt);
        //ease into this fov
        Head.fieldOfView = Mathf.Lerp(Head.fieldOfView, FieldView, 4 * D);
    }

    //slide our character forwards
    void SlideSelf()
    {
        if (lastSlide - Time.time < -SlideTimer)
        {
            //remove any control from player 
            AdjustmentAmt = 0;

            //slide in direction
            Rigid.AddForce(-transform.forward * SlideAmt, ForceMode.Impulse);
            lastSlide = Time.time;
        }
    }

    void SetGravity(float gravScale)
    {
        gravityController.SetGravityScale(gravScale);
    }

    //------------------------------------ Debug and testing features

    void DebugControls()
    {
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            SavePos();
        }

        if (Input.GetKeyDown(KeyCode.Period))
        {
            SetPos();
        }
        //Debug.Log("Speed: " + Rigid.velocity.magnitude);
    }

    void SavePos()
    {
        savePosPlayer = transform.position;
        savePosWorld = Rigid.transform.position;
        Debug.Log("Position saved");
    }

    void SetPos()
    {
        transform.position = savePosPlayer;
        Rigid.transform.position = savePosWorld;
        PlayerRB.velocity = Vector3.zero;
        Rigid.velocity = Vector3.zero;
    }
}
