using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSaveByCollision : Gimmick
{
    [SerializeField] private Transform loadPoint;
    [SerializeField] private Transform signPoint;
    [SerializeField] private Vector3 respawnAngle;
    
    public override void Execute()
    {
        if (!isGimmickEnable) return;

        /* 대폭 변경
         * loadpoint, signpoint 추가
         * loadPoint: 로드할 때(메인화면에서 continue) 풍선의 위치 -> 진정한 의미의 세이브포인트
         * signPoint: 표지판이 떨어질 위치
         * transform.position: 풍선이 리스폰할 때 생성되는 위치
         */
        Vector3 newSavePoint = loadPoint.transform.position; //새로운 세이브포인트
        Vector3 currentSavePoint = GameManager.instance.GetSavePoint(); //기존 세이브포인트

        if (newSavePoint != currentSavePoint)
        {
            GameManager.instance.SetSavePoint(newSavePoint);
            Debug.Log("세이브포인트 변경");
            
            Respawn respawn = FindObjectOfType<Respawn>();
            if (respawn != null)
            {
                respawn.SetRespawnPoint(transform.position);
                respawn.SetRespawnAngle(respawnAngle);
                respawn.SetSavePointReached(true); //세이브포인트 도달 여부 설정
            }

            SetSignPos.instance.UpdateSignPosition(signPoint);
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