using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColorFromTag : MonoBehaviour
{
    private Renderer rend;

    private void Start()
    {
        //Reads the tag of the parent object, then uses the dictionary to convert the string into a usable colour
        rend = GetComponent<Renderer>();
        rend.material.color = ColorDictionary.namesToColors[GetParent().tag];
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
