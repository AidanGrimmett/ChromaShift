using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float mouseSensitivity;
    private GameObject playerCam;
    private Rigidbody worldRB;
    private Rigidbody rb;

    [SerializeField] private float moveSpeed;
    private float xInput;
    private float zInput;

    // Start is called before the first frame update
    void Start()
    {
        //Find objects
        playerCam = GameObject.Find("MainCamera");
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

    private void FixedUpdate()
    {
        if (zInput != 0 || xInput != 0)
        {
            Vector3 normalForward = transform.forward.normalized;

            Vector3 moveForce = normalForward * zInput + transform.right * xInput;

            

            if (false)
            {
                //add the movement force
                rb.AddForce(moveForce * moveSpeed);
            }
            else
            {
                worldRB.AddForce(-moveForce * moveSpeed);
            }
        }
    }

    private void OnDestroy()
    {
        //ensure the mouse is freed when player is destroyed (exiting the game, going to menu etc)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
