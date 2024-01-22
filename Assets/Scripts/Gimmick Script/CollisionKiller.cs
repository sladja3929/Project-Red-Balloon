using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class CollisionKiller : Gimmick
{
    public EffectType effectType;
    
    public override void Execute()
    {
        if (!isGimmickEnable) return;
        
        GameManager.instance.KillBalloon();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!other.collider.CompareTag("Player")) return;
        Execute();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        //play effect
        EffectManager.instance.ShowDeathEffect(effectType, other.transform.position);
        Execute();
    }
}
