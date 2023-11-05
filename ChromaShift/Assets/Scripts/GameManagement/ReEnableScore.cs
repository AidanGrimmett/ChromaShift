using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReEnableScore : MonoBehaviour
{
    public SubmitScore target;

    public void ReEnable()
    {
        target.gameObject.SetActive(true);
        target.submitted = false;
    }
}
