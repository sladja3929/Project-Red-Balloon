using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType
{
    On,
    Off,
    Switch,
    Execute,
}

[Serializable]
public class Command
{
    [SerializeField] private TriggerType triggerType;
    [SerializeField] private Gimmick target;

    public void ExecuteCommand()
    {
        switch(triggerType)
        { 
            case TriggerType.On: 
                target.GimmickOn();
                break;
            
            case TriggerType.Off:
                target.GimmickOff();
                break;
            
            case TriggerType.Switch:
                target.GimmickSwitch();
                break;
            
            case TriggerType.Execute:
                target.Execute();
                break;
            
            default: 
                throw new ArgumentOutOfRangeException();
        }
    }
}