using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOutlineColor : MonoBehaviour
{
    private Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
        outline.OutlineColor = ColorDictionary.namesToColors[transform.parent.gameObject.tag];
        outline.OutlineWidth = 10;
    }
}
