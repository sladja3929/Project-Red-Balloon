using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : Gimmick
{
    public override void Execute()
    {
        GameManager.instance.FinishGame();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) Execute();
    }
}
