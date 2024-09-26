using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotGround : Gimmick
{
    public BalloonDebuff balloonDebuff = null;
    
    [Header("Property")]
    public float heatSpeed;

    private float ss = 1f;
    public override void Execute()
    {
        balloonDebuff.Heat(heatSpeed * Time.deltaTime);
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (!other.collider.CompareTag("Player")) return;

        balloonDebuff = null;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!other.collider.CompareTag("Player")) return;

        balloonDebuff ??= other.collider.GetComponent<BalloonDebuff>();
        
        Execute();
    }
}
