using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredObjectActivator : MonoBehaviour
{
    private bool Active = true;

    private GameObject activeState;
    private GameObject inactiveState;

    private void Start()
    {
        activeState = transform.Find("Active").gameObject;
        inactiveState = transform.Find("Inactive").gameObject;
    }

    private void Update()
    {
        if (ColorDictionary.namesToColors[gameObject.tag] != CurrentColor.currentColor && Active || ColorDictionary.namesToColors[gameObject.tag] == CurrentColor.currentColor && !Active)
        {
            SetActive();
        }
    }

    private void SetActive()
    {
        Active = !Active;
        activeState.SetActive(!Active);
        inactiveState.SetActive(Active);
    }
}
