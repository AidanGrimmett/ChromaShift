using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    //This class is to give the player the ability to cycle through the colors they have access to.

    //Accessible value that stores the color value of the current color the player is on.
    public static Color currentColor;

    //Stores the available colors and the current index.
    private List<Color> colors = new List<Color>();
    private int colorsIndex = 0;

    //Stops the colors from cycling if the player holds down the color input buttons.
    private bool inputPressed = false;

    //Records whether or not the player has pressed the corresponding color changing control.
    private float colorInput;

    private PlayerHealthBarScript playerHealthBar;

    private void Awake()
    {
        //Fill the colors list with available colors.
        PopulateColorList();

        //Sets the current color to be the first color in the list.
        currentColor = colors[colorsIndex];

        playerHealthBar = GetComponent<PlayerHealthBarScript>();
    }

    private void Update()
    {
        //if we have no colour juice, disable colour changing
        if (playerHealthBar.GetHealth() <= 0) return;

        //Changeable input axis read from the project settings -> inputAxes.
        colorInput = Input.GetAxisRaw("Color");

        //When the positive input is pressed, cycle to the next color in the list.
        if (!inputPressed && colorInput > 0.05)
        {
            inputPressed = true;

            colorsIndex++;

            if (colorsIndex == colors.Count)
            {
                colorsIndex = 0;
            }

            currentColor = colors[colorsIndex];
        }

        //When the negative input is pressed, cycle to the previous color in the list.
        if (!inputPressed && colorInput < -0.05)
        {
            inputPressed = true;

            colorsIndex--;

            if (colorsIndex == -1)
            {
                colorsIndex = colors.Count-1;
            }

            currentColor = colors[colorsIndex];
        }


        //When the player stops pressing any input, it resets the player's ability to press the button again
        if (inputPressed && -0.05 < colorInput && 0.05 > colorInput)
        {
            inputPressed = false;
        }
    }

    public int GetColorInt()
    {
        return colorsIndex;
    }    

    //Called at the start of the game. Fills the colors list with the useable color values stored in the dictionary.
    private void PopulateColorList()
    {
        foreach (KeyValuePair<string, Color> entry in ColorDictionary.StringToColorConversion)
        {
            colors.Add(entry.Value);
        }
    }
}
