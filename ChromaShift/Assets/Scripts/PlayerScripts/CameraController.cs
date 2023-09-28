using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float mouseSensitivity;
    private float xRotation;
    private float yRotation;
    private float vertRotation;
    private float horiRotation;

    public Transform orientation;
    

    // Start is called before the first frame update
    void Start()
    {
        mouseSensitivity = 5;// GameObject.Find("Player").GetComponent<PlayerMovementController>().mouseSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;


        vertRotation = -mouseY;
        horiRotation = mouseX;

        xRotation += vertRotation;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        yRotation += horiRotation;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
