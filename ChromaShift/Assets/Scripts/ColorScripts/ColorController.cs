using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    [SerializeField] Color[] colors = { Color.white };
    private int colorsIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            colorsIndex--;
            if (colorsIndex < 0 )
            {
                colorsIndex = colors.Length-1;
            }

            CurrentColor.SetColor(colors[colorsIndex]);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            colorsIndex++;
            if (colorsIndex == colors.Length)
            {
                colorsIndex = 0;
            }

            CurrentColor.SetColor(colors[colorsIndex]);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            CurrentColor.SetColor(Color.white);
        }
    }
}
