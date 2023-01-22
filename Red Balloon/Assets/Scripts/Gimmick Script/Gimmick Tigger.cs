using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GimmickTigger : Gimmick
{
    [SerializeField] private Gimmick targetGimmick;

    private enum TriggerType
    {
        On,
        Off,
        Switch,
        Execute,
    }

    [SerializeField] private TriggerType triggerType;
    public override void Execute()
    {
        if (!isGimmickEnable) return;
        
        switch(triggerType)
        { 
            case TriggerType.On: 
                targetGimmick.GimmickOn();
                break;
            
            case TriggerType.Off:
                targetGimmick.GimmickOff();
                break;
            
            case TriggerType.Switch:
                targetGimmick.GimmickSwitch();
                break;
            
            case TriggerType.Execute:
                targetGimmick.Execute();
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        Execute();
    }
}
