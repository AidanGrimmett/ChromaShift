using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    [SerializeField] Color[] colors = { Color.white };
    private int colorsIndex = 0;

    private void Update()
    {
        //When the player presses Q, it cycles to the next colour
        if (Input.GetKeyDown(KeyCode.Q))
        {
            colorsIndex--;
            if (colorsIndex < 0 )
            {
                colorsIndex = colors.Length-1;
            }

            CurrentColor.SetColor(colors[colorsIndex]);
        }
        //When the player presses E, it cycles to the previous colour
        if (Input.GetKeyDown(KeyCode.E))
        {
            colorsIndex++;
            if (colorsIndex == colors.Length)
            {
                colorsIndex = 0;
            }

            CurrentColor.SetColor(colors[colorsIndex]);
        }
        //When the player presses F, it resets back to the default colour(white)
        if (Input.GetKeyDown(KeyCode.F))
        {
            CurrentColor.SetColor(Color.white);
        }
    }
}
