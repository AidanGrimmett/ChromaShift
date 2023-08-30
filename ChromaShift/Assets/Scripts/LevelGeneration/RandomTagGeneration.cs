using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTagGeneration : MonoBehaviour
{
    public static string lastChosenColor { get; private set; }

    private List<string> colors = new List<string>();
    private void Awake()
    {
        foreach (var color in ColorDictionary.colorToName)
        {
            colors.Add(color.Value);
        }
        colors.Remove("White");

        int colorIndex = ChooseRandomColor();
        
        gameObject.tag = colors[colorIndex];
        lastChosenColor = colors[colorIndex];
    }

    private int ChooseRandomColor()
    {
        int index = Random.Range(0, colors.Count);
        int count = 0;

        while (colors[index] == lastChosenColor || count < 10)
        {
            index = Random.Range(0, colors.Count);
            count++;
        }

        return index;
    }
}
