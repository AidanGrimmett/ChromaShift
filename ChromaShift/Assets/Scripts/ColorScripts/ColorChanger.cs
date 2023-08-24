using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (rend.material.color != CurrentColor.currentColor)
        {
            rend.material.color = CurrentColor.currentColor;
        }
    }
}
