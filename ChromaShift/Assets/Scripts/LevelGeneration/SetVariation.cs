using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVariation : MonoBehaviour
{
    [SerializeField] private GameObject Style1;
    [SerializeField] private GameObject Style2;
    [SerializeField] private GameObject Style3;

    private void Start()
    {
        if (Score.score > 0 )
        {
            Style1.SetActive(false);
            Style2.SetActive(false);
            Style3.SetActive(true);
        }
        else if (Score.score > 3 )
        {
            Style1.SetActive(false);
            Style2.SetActive(true);
            Style3.SetActive(false);
        }
        else
        {
            Style1.SetActive(true);
            Style2.SetActive(false);
            Style3.SetActive(false);
        }
    }
}
