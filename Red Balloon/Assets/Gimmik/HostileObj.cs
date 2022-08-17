using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileObj : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player crushed");
        }
    }
}
