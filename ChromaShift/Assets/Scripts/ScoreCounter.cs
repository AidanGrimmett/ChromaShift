using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    private static int score;

    private void Start()
    {
        score = 0;
    }

    public static void AddScore()
    {
        score++;
    }

    public static int GetScore()
    {
        return score;
    }

    public static void ResetScore()
    {
        score=0;
    }
}
