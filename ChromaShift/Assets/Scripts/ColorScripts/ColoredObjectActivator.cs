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
        //Check the game to see which object should be active at the start of the game
        if (activeState.activeSelf && inactiveState.activeSelf)
        {
            if (ColorDictionary.namesToColors[gameObject.tag] == CurrentColor.currentColor)
            {
                SetActive();
            }
        }

        //Check to see if the game state has been updated and switches the active and inactive object's state
        if (ColorDictionary.namesToColors[gameObject.tag] != CurrentColor.currentColor && Active || ColorDictionary.namesToColors[gameObject.tag] == CurrentColor.currentColor && !Active)
        {
            SetActive();
        }
    }

    private void SetActive()
    {
        //Switches which child object is currenty active
        Active = !Active;
        activeState.SetActive(!Active);
        inactiveState.SetActive(Active);
    }
}
