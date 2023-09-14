using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangerRenderer : MonoBehaviour
{
    private Renderer rend;

    //Allows the opacity to be set (useful for the filter object)
    [SerializeField] private float opacity = 1f;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        //Checks if the colour matches the current colour set by the player.
        if (rend.material.color != CurrentColor.currentColor)
        {
            rend.material.color = new Color(CurrentColor.currentColor.r, CurrentColor.currentColor.g, CurrentColor.currentColor.b, opacity);
        }
    }
}
