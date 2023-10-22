using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GimmickTrigger : Gimmick
{
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
