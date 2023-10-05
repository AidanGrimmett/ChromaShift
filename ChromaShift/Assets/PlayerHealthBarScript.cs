using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarScript : MonoBehaviour
{
    private CapsuleCollider collider;

    private void Start()
    {
        collider = GetComponent<CapsuleCollider>();
    }
}
