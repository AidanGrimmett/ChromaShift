using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteSpinner : MonoBehaviour
{
    public ColorController cc;

    private List<float> positionList = new List<float> {0, 120, 240};

    bool isRotating;
    private Quaternion targetRotation;

    public float rotationSpeed;
    public float rotationThreshold;

    private void LateUpdate()
    {
        // Check for user input or some condition to trigger rotation
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if (isRotating)
            {
                QuickSetRotation();
                isRotating = false;
            }
            else
            {
                targetRotation = Quaternion.Euler(-90, positionList[cc.GetColorInt()], 0);
                Debug.Log("Spinner Color index:  " + cc.GetColorInt());
                isRotating = true;
            }
        }

        if (isRotating)
        {
            // Perform smooth rotation towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if the rotation is close enough to the target
            if (Quaternion.Angle(transform.localRotation, targetRotation) < rotationThreshold)
            {
                // Rotation is considered finished
                transform.localRotation = targetRotation;
                isRotating = false;
            }
    }

    void QuickSetRotation()
    {
        transform.localRotation = targetRotation;
    }
}
}
