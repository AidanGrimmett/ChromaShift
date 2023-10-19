using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteSpinner : MonoBehaviour
{
    public float rotationSpeed = 60.0f; // Adjust the speed of rotation

    private bool isRotating = false;
    private float targetRotation = 0.0f;

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
