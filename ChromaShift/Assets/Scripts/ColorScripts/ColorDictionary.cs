
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDictionary : MonoBehaviour
{
    //This class is used to store the color to name conversion dictionaries.
    //All other classes will refer to these static dictionaries in order to convert between the color values and the names of the color.

    public static Dictionary<string, Color> StringToColorConversion = new Dictionary<string, Color>()
    {
        { "Red", Color.red },
        { "Green", Color.green },
        { "Blue", Color.blue }
    };

    public static Dictionary<Color, string> ColorToStringConversion = new Dictionary<Color, string>()
    {
        { Color.red, "Red" },
        { Color.green, "Green" },
        { Color.blue, "Blue" }

    };
}
