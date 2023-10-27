using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] private UnityEvent onTrigger;

    //This is what generates a chunk when we pass through a trigger collider
    private void OnTriggerEnter(Collider other)
    {
        onTrigger.Invoke();
        Destroy(gameObject);
    }
}
