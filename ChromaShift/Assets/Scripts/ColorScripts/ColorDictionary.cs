using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDictionary : MonoBehaviour
{
    //Used to convert colour to string
    public static Dictionary<Color, string> colorToName = new Dictionary<Color, string>()
    {
        {Color.red, "Red"},
        {Color.green, "Green" },
        {Color.blue, "Blue" },
        {Color.white, "White" }
    };
    public static Dictionary<string, Color> namesToColors = new Dictionary<string, Color>()
    {
        //Used to convert string to colour
        {"Red", Color.red},
        {"Green", Color.green},
        {"Blue", Color.blue},
        {"White", Color.white }
    };
}
