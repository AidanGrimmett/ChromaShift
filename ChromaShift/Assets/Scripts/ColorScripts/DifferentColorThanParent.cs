using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferentColorThanParent : MonoBehaviour
{
    private List<string> colors = new List<string>();

    void Start()
    {
        string parentColor = GetParent().tag;

        foreach (var color in ColorDictionary.colorToName)
        {
            colors.Add(color.Value);
        }
        colors.Remove("White");
        colors.Remove(parentColor);

        int index = Random.Range(0, colors.Count);

        gameObject.tag = colors[index];
    }

    private GameObject GetParent()
    {
        Transform parent;
        parent = transform.parent;
        while (parent.transform.tag == "Untagged")
        {
            parent = parent.transform.parent;
        }

        return parent.gameObject;
    }
}
