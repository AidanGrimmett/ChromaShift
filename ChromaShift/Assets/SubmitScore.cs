using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitScore : MonoBehaviour
{
    public bool submitted;

    [SerializeField] private Leaderboard leaderboard;
    [SerializeField] private InputField input;

    public void SendScore(string name)
    {
        leaderboard.AddHighscoreEntry(Score.score, Score.time, name);
    }

    private void Update()
    {
        if (submitted)
        {
            input.gameObject.SetActive(false);
        }
    }
}
