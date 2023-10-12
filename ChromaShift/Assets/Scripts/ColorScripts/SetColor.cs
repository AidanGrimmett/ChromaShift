using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour
{
    //Sets the color of the object to be the randomly assigned color generated from the tag

    //Different renderers are required depending if it is 3D or 2D
    private SpriteRenderer rend2D;
    private Renderer rend3D;

    private void Start()
    {
        rend2D = GetComponent<SpriteRenderer>();
        rend3D = GetComponent<Renderer>();

        if (rend2D != null )
        {
            Set2DColor();
        }

        if (rend3D != null )
        {
            Set3DColor();
        }

        CreateController();
    }

    private void Set2DColor()
    {
        rend2D.color = ColorDictionary.StringToColorConversion[transform.tag];
    }

    private void Set3DColor()
    {
        Material[] materials = rend3D.materials;

        foreach ( Material mat in materials )
        {
            Debug.Log(mat.name);
            if (mat.name == "Laser (Instance)")
            {
                mat.SetColor("_EmissionColor", ColorDictionary.StringToColorConversion[transform.tag] * 20);
            }
            else
            {
                mat.color = ColorDictionary.StringToColorConversion[transform.tag];
            }
        }
    }

    private void CreateController()
    {
        GameObject parentObject = new GameObject(); //create an 'empty' object
        parentObject.name = "Controller";
        parentObject.transform.parent = transform.parent;
        transform.parent = parentObject.transform;
        ColorObjectManager controller = parentObject.AddComponent<ColorObjectManager>();
        controller.SetTargets(ColorDictionary.StringToColorConversion[transform.tag], gameObject);
    }
}
