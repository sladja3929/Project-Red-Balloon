using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GimmickTrigger : Gimmick
{
    [SerializeField] private List<Command> commands;
    [SerializeField] private bool debug = false;
    public override void Execute()
    {
        if (!isGimmickEnable) return;
        
        if (debug)
        {
            Debug.Log("Gimmick Trigger Call");
        }

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
