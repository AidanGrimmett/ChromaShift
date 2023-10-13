using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteSpinner : MonoBehaviour
{
    private float targetRotation;
    private Transform trans;

    private Vector3 currentRotation;
    private Vector3 newRotation;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AddRotation(120);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            AddRotation(-120);
        }

        transform.localEulerAngles = (Vector3.Lerp(trans.rotation.eulerAngles, newRotation, 0.5f));
    }

    void AddRotation(float rotationAmount)
    {
        // Calculate the new rotation by adding the rotationAmount to the current rotation
        Vector3 currentRotation = transform.rotation.eulerAngles;
        Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y + rotationAmount, currentRotation.z);
    }
}
