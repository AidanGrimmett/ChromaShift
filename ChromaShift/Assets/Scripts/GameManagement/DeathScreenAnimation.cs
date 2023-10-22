using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreenAnimation : MonoBehaviour
{
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private GameObject spotlight;
    [SerializeField] private GameObject noSpotlight;
    [SerializeField] private GameObject[] elements;

    private void OnEnable()
    {
        noSpotlight.SetActive(true);
        spotlight.SetActive(false);
        setButtonActive(false);
        Debug.Log("Switch");
        StartCoroutine(Switch());
    }

    private IEnumerator Switch()
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Switching");

        noSpotlight.SetActive(false);
        spotlight.SetActive(true);
        yield return new WaitForSeconds(delay);
        setButtonActive(true);
    }

    private void setButtonActive(bool val)
    {
        foreach(GameObject element in elements)
        {
            element.SetActive(val);
        }
    }
}
