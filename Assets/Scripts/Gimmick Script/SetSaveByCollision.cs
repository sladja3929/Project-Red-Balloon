using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSaveByCollision : Gimmick
{
    [SerializeField] private Transform savePoint;
    public override void Execute()
    {
        if (!isGimmickEnable) return;
        
        GameManager.instance.SetSavePoint(savePoint.position);
        Respawn respawn = FindObjectOfType<Respawn>();
        if (respawn != null)
        {
            respawn.SetSavePoint(savePoint.position);
            respawn.SetSavePointReached(true);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.collider.CompareTag("Player")) return;

        Execute();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        Execute();
    }
}
