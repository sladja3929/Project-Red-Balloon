using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSaveByCollision : Gimmick
{
    private void Start()
    {
        if (TempSignManager.instance == null)
        {
            new GameObject("TempSignManager", typeof(TempSignManager));
        }
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
            if (respawn != null)
            {
                Debug.Log("세이브포인트 변경");
                respawn.SetSavePoint(newSavePoint);
                respawn.SetSavePointReached(true);
                TempSignManager.instance.IncrementSavePointIndex();
                respawn.SetSignPosIndex(TempSignManager.instance.GetSavePointIndex());
            }

            SetSignPos setSignPos = FindObjectOfType<SetSignPos>();
            if (setSignPos != null)
            {
                if (!setSignPos.CheckUpdateSignPos(TempSignManager.instance.GetSavePointIndex()))
                {
                    Debug.Log("세이브포인트 복구");
                    TempSignManager.instance.DecrementSavePointIndex();
                }
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
