using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChunkTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent onTrigger;

    private void OnTriggerEnter(Collider other)
    {
        onTrigger.Invoke();
        Destroy(gameObject);
    }
}
