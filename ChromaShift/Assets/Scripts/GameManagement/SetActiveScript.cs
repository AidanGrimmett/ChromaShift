using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveScript : MonoBehaviour
{
    public void DeactivateObject()
    {
        gameObject.SetActive(false);
    }
}
