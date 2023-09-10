using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOutlineColor : MonoBehaviour
{
    private Outline outline;

    private void Start()
    {
        //Sets up the outline of each of the inacitve objects
        outline = GetComponent<Outline>();
        outline.OutlineColor = ColorDictionary.namesToColors[GetParent().tag];
        outline.OutlineWidth = 10;
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
