using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentColor : MonoBehaviour
{
    //Keeps track of the current colour the player has set it to, this can be called without being attached to the object.
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
