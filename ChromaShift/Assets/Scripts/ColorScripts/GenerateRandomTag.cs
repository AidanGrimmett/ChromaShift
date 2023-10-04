using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRandomTag : MonoBehaviour
{
    //This class is to be used on objects that need to be colored. 

    //Tick this if a tag needs to be assigned to child objects
    [SerializeField] bool generateTagsForChildren;
    [SerializeField] Transform[] children;


    //Stores the names of the available colors.
    private List<string> colors = new List<string>();

    private void Awake()
    {
        //Fills the color list with all available colors
        PopulateColorList();

        //Assigns a random color tag
        transform.tag = GetRandomColorTag();

        if (generateTagsForChildren)
        {

            AssignTagsToChildren();
        }
    }

    //Called at the start of the game. Fills the colors list with the useable color values stored in the dictionary.
    private void PopulateColorList()
    {
        foreach (KeyValuePair<Color, string> entry in ColorDictionary.ColorToStringConversion)
        {
            colors.Add(entry.Value);
        }
    }

    //Returns a random tag from the colors list.
    private string GetRandomColorTag()
    {
        //Choose random index.
        int randomIndex = Random.Range(0, colors.Count);

        return colors[randomIndex];
    }

    //Iterate through the list of children, assigning different tags.
    private void AssignTagsToChildren()
    {
        colors.Remove(transform.tag);
        foreach (Transform child in children)
        {
            child.tag = GetRandomColorTag();
        }
        colors.Add(transform.tag);
    }
}
