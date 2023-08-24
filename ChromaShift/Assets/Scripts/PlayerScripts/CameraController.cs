using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float mouseSensitivity;
    private float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        mouseSensitivity = GetComponentInParent<PlayerMovementController>().mouseSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y");

        float vertRotation = -mouseY * mouseSensitivity;
        xRotation += vertRotation;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        transform.rotation = Quaternion.Euler(xRotation, transform.eulerAngles.y, 0f);
    }
}
