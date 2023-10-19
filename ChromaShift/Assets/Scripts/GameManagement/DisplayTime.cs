using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTime : MonoBehaviour
{
    [SerializeField] private Text score;

    private void Update()
    {
        score.text = "Time: " + Score.time.ToString("n2");
    }
}
