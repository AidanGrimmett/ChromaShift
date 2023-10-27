using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitScore : MonoBehaviour
{
    public bool submitted;

    [SerializeField] private InputField mainInputField;

    [SerializeField] private Leaderboard leaderboard;

    public void SendScore(string name)
    {
        leaderboard.AddHighscoreEntry(Score.score, Score.time, name);
        submitted = true;
    }

    private void Start()
    {
        mainInputField.onEndEdit.AddListener(SendScore);
    }

    private void Update()
    {
        if (submitted)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void Unsubmit()
    {
        submitted = false;
    }
}
