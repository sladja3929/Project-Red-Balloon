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

        Vector3 newSavePoint = transform.position;//새로운 세이브포인트
        Vector3 currentSavePoint = GameManager.instance.GetSavePoint();//기존 세이브포인트

        if (newSavePoint != currentSavePoint)
        {
            GameManager.instance.SetSavePoint(newSavePoint);

            Respawn respawn = FindObjectOfType<Respawn>();
            if (respawn != null)
            {
                Debug.Log("세이브포인트 변경");
                respawn.SetSavePoint(newSavePoint);//이거 없어도 될거같은데 작동되니까 냅둠
                respawn.SetSavePointReached(true);//세이브포인트 도달 여부 설정
                TempSignManager.instance.IncrementSavePointIndex();//세이브포인트 인덱스 증가(다음 위치)
                respawn.SetSignPosIndex(TempSignManager.instance.GetSavePointIndex());//세이브포인트 인덱스 설정
            }

            SetSignPos setSignPos = FindObjectOfType<SetSignPos>();
            if (setSignPos != null)
            {
                if (!setSignPos.CheckUpdateSignPos(TempSignManager.instance.GetSavePointIndex()))//만약 업데이트 하면 안되면 복구
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
