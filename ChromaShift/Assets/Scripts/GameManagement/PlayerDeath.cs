using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public static void KillPlayer()
    {
        GameObject.Find("Screens").GetComponent<MenuManager>().gameState = GameState.End;
        AudioManager.instance.PlaySound("laser");
    }
}
