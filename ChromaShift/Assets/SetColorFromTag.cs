using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColorFromTag : MonoBehaviour
{
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = ColorDictionary.namesToColors[transform.parent.gameObject.tag];
    }
}
