using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSaveByCollision : Gimmick
{
    private int originalIndex;

    private void Start()
    {
    }
    public override void Execute()
    {
        if (!isGimmickEnable) return;

        Vector3 newSavePoint = transform.position;
        Vector3 currentSavePoint = GameManager.instance.GetSavePoint();

        if (newSavePoint != currentSavePoint)
        {
            GameManager.instance.SetSavePoint(newSavePoint);

            Respawn respawn = FindObjectOfType<Respawn>();
            if (TempSignManager.instance != null)
            {
                originalIndex = TempSignManager.instance.GetSavePointIndex();
                TempSignManager.instance.IncrementSavePointIndex();
                Debug.Log("SavePointIndex: " + TempSignManager.instance.GetSavePointIndex() + " Original: " + originalIndex);
                if (respawn != null)
                {
                    respawn.SetSavePoint(newSavePoint);
                    respawn.SetSavePointReached(true);
                    respawn.SetSignPosIndex(TempSignManager.instance.GetSavePointIndex());
                }

                SetSignPos setSignPos = FindObjectOfType<SetSignPos>();
                if (setSignPos != null)
                {
                    if (!setSignPos.CheckUpdateSignPos(TempSignManager.instance.GetSavePointIndex()))
                        TempSignManager.instance.DecrementSavePointIndex();
                }
            }
            else
            {
                Debug.LogError("TempSignManager instance is null.");
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.collider.CompareTag("Player")) return;

        Execute();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        Execute();
    }
}
