using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    [SerializeField] Color[] colors = { Color.white };
    private int colorsIndex = 0;

    private bool inputPressed;

    private void Update()
    {
        float input = Input.GetAxisRaw("ChangeColor");
        //When the player presses Q, it cycles to the next colour
        if (input < 0 && !inputPressed)
        {
            inputPressed = true;
            colorsIndex--;
            if (colorsIndex < 0 )
            {
                colorsIndex = colors.Length-1;
            }

            CurrentColor.SetColor(colors[colorsIndex]);
        }
        //When the player presses E, it cycles to the previous colour
        if (input > 0 && !inputPressed)
        {
            inputPressed = true;
            colorsIndex++;
            if (colorsIndex == colors.Length)
            {
                colorsIndex = 0;
            }

            CurrentColor.SetColor(colors[colorsIndex]);
        }
        
        if (input < 0.05 && input > -0.05 && inputPressed)
        {
            inputPressed = false;
        }
    }
}
