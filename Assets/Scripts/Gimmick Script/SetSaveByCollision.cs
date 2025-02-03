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

        Vector3 newSavePoint = transform.position;//���ο� ���̺�����Ʈ
        Vector3 currentSavePoint = GameManager.instance.GetSavePoint();//���� ���̺�����Ʈ

        if (newSavePoint != currentSavePoint)
        {
            GameManager.instance.SetSavePoint(newSavePoint);

            Respawn respawn = FindObjectOfType<Respawn>();
            if (respawn != null)
            {
                Debug.Log("���̺�����Ʈ ����");
                respawn.SetSavePoint(newSavePoint);//�̰� ��� �ɰŰ����� �۵��Ǵϱ� ����
                respawn.SetSavePointReached(true);//���̺�����Ʈ ���� ���� ����
                TempSignManager.instance.IncrementSavePointIndex();//���̺�����Ʈ �ε��� ����(���� ��ġ)
                respawn.SetSignPosIndex(TempSignManager.instance.GetSavePointIndex());//���̺�����Ʈ �ε��� ����
            }

            SetSignPos setSignPos = FindObjectOfType<SetSignPos>();
            if (setSignPos != null)
            {
                if (!setSignPos.CheckUpdateSignPos(TempSignManager.instance.GetSavePointIndex()))//���� ������Ʈ �ϸ� �ȵǸ� ����
                {
                    Debug.Log("���̺�����Ʈ ����");
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