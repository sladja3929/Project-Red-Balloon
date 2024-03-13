using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneFall : Gimmick
{
    // Shoot Ray cast, and if it hits the player, set rigidbody's kinematic to false
    
    public GameObject stone;

    public void Update()
    {
        if (RayCast())
        {
            stone.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    
    private void OnCollisionEnter(Collider other)
    {
        //kill player
        if (other.CompareTag("Player"))
        {
            GameManager.instance.KillBalloon();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //kill player
        if (other.CompareTag("Player"))
        {
            GameManager.instance.KillBalloon();
        }
    }
    
    public bool RayCast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
}
