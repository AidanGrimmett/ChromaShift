using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public static void KillPlayer()
    {
        GameObject.Find("GameplayUI").GetComponent<ScreenFlashAnimation>().FlashDeath();
        AudioManager.instance.PlaySound("laser");
        MenuManager screens = GameObject.Find("Screens").GetComponent<MenuManager>();
        screens.StartCoroutine(screens.screenSwitchDelay());
        MenuManager.gameState = GameState.End;
        AudioManager.instance.StopSound("music");
    }
}
