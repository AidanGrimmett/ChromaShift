using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;

    [Header("Drag")]
    [SerializeField] private float groundDrag;
    [SerializeField] private float airDrag;
    [SerializeField] private float wallDrag;

    [Header("Collision Checks")]
    private bool isGrounded;
    private bool isOnWall;
    [SerializeField] private LayerMask groundLayer;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    Rigidbody worldRB;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        worldRB = GameObject.Find("World").GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }


    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.2f, groundLayer);
        isOnWall = Physics.Raycast(transform.position, Vector3.right, transform.localScale.x * 0.5f + 0.2f, groundLayer);
        Debug.Log(isGrounded);

        GetInput();

        if (isGrounded)
        {
            worldRB.drag = groundDrag;
        }
        else if (false)
        {
            worldRB.drag = wallDrag;
        }
        else
        {
            worldRB.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = transform.forward * verticalInput + transform.right * (horizontalInput / 2);

        float moveForce = moveSpeed - ((new Vector3(worldRB.velocity.x, 0, worldRB.velocity.z).magnitude / maxSpeed) * 100);
        worldRB.AddForce(-moveDirection.normalized * moveForce, ForceMode.Force);
        LimitSpeed();
    }

    private void LimitSpeed()
    {
        Vector3 flatVel = new Vector3(worldRB.velocity.x, 0, worldRB.velocity.z);

        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            worldRB.velocity = new Vector3(limitedVel.x, worldRB.velocity.y, limitedVel.z);
        }
    }
}
