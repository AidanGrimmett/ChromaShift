using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentColor : MonoBehaviour
{
    public static Color currentColor { get; private set; }

    public void Start()
    {
        currentColor = Color.white;
    }

    public static void SetColor(Color clr)
    {
        currentColor = clr;
    }
}
