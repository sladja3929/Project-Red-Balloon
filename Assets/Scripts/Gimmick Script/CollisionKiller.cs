using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class CollisionKiller : Gimmick
{
    public EffectType effectType;

    private bool activating = false;
    private float effectDelay = 0f;
    private float deathDelay = 0f;
    
    private void Awake()
    {
        if (effectType == EffectType.Shark)
        {
            effectDelay = 3f;
            deathDelay = 3.2f;
        }
    }
    public override void Execute()
    {
        if (!isGimmickEnable) return;
        
        GameManager.instance.KillBalloon();
        activating = false;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!other.collider.CompareTag("Player")) return;
        if (activating) return;

        activating = true;
        GameManager.instance.CanSuicide = false;
        StartCoroutine(EffectManager.instance.ShowDeathEffectCoroutine(effectType, effectDelay));
        Invoke("Execute", deathDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (activating) return;
        
        activating = true;
        GameManager.instance.CanSuicide = false;
        StartCoroutine(EffectManager.instance.ShowDeathEffectCoroutine(effectType, effectDelay));
        Invoke("Execute", deathDelay);
    }
}
