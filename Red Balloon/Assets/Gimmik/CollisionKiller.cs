using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionKiller : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (!other.collider.CompareTag("Player")) return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
    }
}
