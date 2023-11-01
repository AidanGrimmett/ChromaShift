using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int score = 0;
    public static float time = 0;

    [SerializeField] private GameObject menuManager;

    public void  ResetScores()
    {
        score = 0;
        time = 0;
    }

    public void AddScore()
    {
        score++;
    }

    private void Update()
    {
        if (MenuManager.gameState == GameState.Play)
        {
            time += Time.deltaTime;
        }
    }
}
