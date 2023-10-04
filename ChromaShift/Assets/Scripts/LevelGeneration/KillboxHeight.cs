using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillboxHeight : MonoBehaviour
{
    [SerializeField] float minHeight = 8;
    [SerializeField] float maxHeight = 10;

    private void Update()
    {
        bool tooHigh = Physics.Raycast(transform.position, Vector3.up, minHeight, LayerMask.GetMask("Environment"));
        bool tooLow = !Physics.Raycast(transform.position, Vector3.up, maxHeight, LayerMask.GetMask("Environment"));
        bool underPlatform = Physics.Raycast(transform.position, Vector3.up, LayerMask.GetMask("Environment"));
        bool underPlayer = Physics.Raycast(transform.position, Vector3.up);

        if (!tooLow && !tooHigh || !underPlatform || underPlayer)
        {
            return;
        }

        if (underPlatform && tooLow)
        {
            transform.position += Vector3.up * 0.5f;
        }
        else if (underPlatform && tooHigh)
        {
            transform.position += Vector3.down * 0.5f;
        }
    }
}
