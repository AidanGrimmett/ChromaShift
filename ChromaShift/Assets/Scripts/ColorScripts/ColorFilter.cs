using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorFilter : MonoBehaviour
{
    private Image img;

    //Allows the opacity to be set (useful for the filter object)
    [SerializeField] private float opacity = 1f;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    private void Update()
    {
        //Checks if the colour matches the current colour set by the player.
        if (img.color != CurrentColor.currentColor)
        {
            img.color = new Color(CurrentColor.currentColor.r, CurrentColor.currentColor.g, CurrentColor.currentColor.b, opacity);
        }
    }
}
