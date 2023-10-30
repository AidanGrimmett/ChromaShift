using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObjectManager : MonoBehaviour
{
    private Color activeColor;
    private GameObject coloredObject;

    private void Update()
    {
        if (ColorController.currentColor != activeColor)
        {
            SetActive(true);
        }
        else if (ColorController.currentColor == activeColor)
        {
            SetActive(false);
        }
    }

    public void SetTargets(Color col, GameObject obj)
    {
        activeColor = col;
        coloredObject = obj;
    }

    private void SetActive(bool state)
    {
        if (coloredObject != null)
        {
            coloredObject.SetActive(state);
        }
    }
}
