using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSaveByCol : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (!other.collider.CompareTag("Player")) return;

        other.collider.GetComponent<Respawn>().SetSavePoint(transform.position);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        other.GetComponent<Respawn>().SetSavePoint(transform.position);
    }
}
