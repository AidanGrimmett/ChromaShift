using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDictionary : MonoBehaviour
{
    public static Dictionary<Color, string> colorToName = new Dictionary<Color, string>()
    {
        {Color.red, "Red"},
        {Color.green, "Green" },
        {Color.blue, "Blue" }
    };
    public static Dictionary<string, Color> namesToColors = new Dictionary<string, Color>()
    {
        {"Red", Color.red},
        {"Green", Color.green},
        {"Blue", Color.blue}
    };
}
