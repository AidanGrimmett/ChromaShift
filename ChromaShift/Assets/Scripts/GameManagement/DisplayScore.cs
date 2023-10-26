using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour
{
    [SerializeField] private Text score;

    private void Update()
    {
        score.text = "Labs: " + Score.score;
    }
}
