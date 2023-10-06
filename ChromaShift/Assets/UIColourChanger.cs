using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIColourChanger : MonoBehaviour
{
    [SerializeField] GameObject red;
    [SerializeField] GameObject green;
    [SerializeField] GameObject blue;

    // Update is called once per frame
    void Update()
    {
        string color = ColorDictionary.ColorToStringConversion[ColorController.currentColor];

        if (color.ToLower() == "red")
        {
            deactivateAll();
            red.SetActive(true);
        }
        else if (color.ToLower() == "green")
        {
            deactivateAll();
            green.SetActive(true);
        }
        else if (color.ToLower() == "blue")
        {
            deactivateAll();
            blue.SetActive(true);
        }
    }

    void deactivateAll()
    {
        red.SetActive(false);
        green.SetActive(false);
        blue.SetActive(false);
    }
}
