using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteSpinner : MonoBehaviour
{
    //private Quaternion targetRotation;

    //private Vector3 currentRotation;
    //private Vector3 newRotation;

    //private bool isRotating;

    public float rotationSpeed = 60.0f; // Adjust the speed of rotation

    private bool isRotating = false;
    private float targetRotation = 0.0f;

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        AddRotation(120);
    //        Debug.Log("MB 1");
    //    }
    //    else if (Input.GetMouseButtonDown(1))
    //    {
    //        AddRotation(-120);
    //    }

    //    if (isRotating)
    //    {
    //        Debug.Log("Is rotating");
    //        Debug.Log("Local rotation: " + transform.localRotation + "target: " + targetRotation);
    //        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, 0.5f);

    //        if (Quaternion.Angle(transform.localRotation, targetRotation) < 1f)
    //        {
    //            transform.localRotation = targetRotation;
    //            isRotating = false;
    //            Debug.Log("Stopped");
    //        }
    //    }
    //}

    void AddRotation(float rotationAmount)
    {
        // Calculate the new rotation by adding the rotationAmount to the current rotation
        //targetRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + rotationAmount, transform.localRotation.z);
        //isRotating = true;
    }


    private void Update()
    {
        // Check for user input or some condition to trigger rotation
        if (Input.GetMouseButtonDown(0))
        {
            if (!isRotating)
            {
                StartCoroutine(RotateSmoothly(120.0f)); // Rotate by 120 degrees
            }
            else 
            {
                QuickSetRotation();
                StartCoroutine(RotateSmoothly(120.0f));
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (!isRotating)
            {
                StartCoroutine(RotateSmoothly(-120.0f)); // Rotate by 120 degrees
            }
            else
            {
                QuickSetRotation();
                StartCoroutine(RotateSmoothly(-120.0f));
            }
        }
    }
    IEnumerator RotateSmoothly(float degrees)
    {
        isRotating = true;
        float startRotation = transform.eulerAngles.z;
        targetRotation = startRotation + degrees;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * (rotationSpeed / 60.0f); // Adjust for frame rate independence

            // Perform the rotation
            transform.localRotation = Quaternion.Euler(-90, Mathf.Lerp(startRotation, targetRotation, t), 0);

            yield return null;
        }

        isRotating = false;
    }

    void QuickSetRotation()
    {
        transform.localRotation = Quaternion.Euler(-90, targetRotation, 0);
    }
}
