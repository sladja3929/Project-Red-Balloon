using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSaveByCollision : Gimmick
{
    [SerializeField] private Transform savePoint;
    public override void Execute()
    {
        if (!isGimmickEnable) return;
        
        GameManager.Instance.SetSavePoint(savePoint.position);
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
