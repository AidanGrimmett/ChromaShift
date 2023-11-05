using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserwallAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //AudioManager.instance.PlayPositionalRepeat("laserbuzz", transform.position, "laserBuzzB");
    }

    private void OnEnable()
    {
        //AudioManager.instance.PlayPositional("laserbuzz", transform.position);
    }

    private void OnDisable()
    {
        //AudioManager.instance.RemoveRepeat("laserbuzzB");
    }
}
