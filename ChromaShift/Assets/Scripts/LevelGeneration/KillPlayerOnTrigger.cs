using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject screenManager = GameObject.Find("ScreenManager");

        screenManager.GetComponent<MenuManager>().GameOver();
    }
}
