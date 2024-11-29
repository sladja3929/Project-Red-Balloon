using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotGround : Gimmick
{
    [SerializeField] private float heatingPower;
    private BalloonDebuff balloonDebuff = null;

    public override void Execute()
    {
        
    }

    private void OnCollisionExit(Collision other)
    {
        if (!other.collider.CompareTag("Player")) return;

        balloonDebuff.ColdBalloon();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.collider.CompareTag("Player")) return;

        balloonDebuff ??= other.collider.GetComponent<BalloonDebuff>();
        
        balloonDebuff.enabled = true;
        balloonDebuff.HeatBalloon(heatingPower);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        balloonDebuff.ColdBalloon();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        balloonDebuff ??= other.GetComponent<BalloonDebuff>();
        
        balloonDebuff.enabled = true;
        balloonDebuff.HeatBalloon(heatingPower);
    }
}
