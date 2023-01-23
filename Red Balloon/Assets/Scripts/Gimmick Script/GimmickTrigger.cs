using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GimmickTrigger : Gimmick
{
    private enum TriggerType
    {
        On,
        Off,
        Switch,
        Execute,
    }
    [Serializable]
    private class Command
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

    [SerializeField] private List<Command> commands;
    public override void Execute()
    {
        if (!isGimmickEnable) return;

        foreach (var command in commands)
        {
            command.ExecuteCommand();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        Execute();
    }
}
