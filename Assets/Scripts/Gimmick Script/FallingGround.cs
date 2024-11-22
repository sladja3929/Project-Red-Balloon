using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FallingGround : Gimmick
{
    private Rigidbody _rigid;

    private GameObject _groundThatWillFall;
    [SerializeField] private float respawnTime;
    [SerializeField] private float fallTime;
    [SerializeField] private float randomRotationSpeed;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _groundThatWillFall = gameObject;
    }
    
    private bool _isBreaking;
    public override void Execute()
    {
        if (!isGimmickEnable) return;
        
        _isBreaking = true;
        Invoke(nameof(Fall), fallTime);
    }

    private void OnCollisionStay(Collision collision)
    {
        //player와 충돌했으며, 복사본이 아니고 이미 Fall 함수가 실행 된 상태가 아니면 Execute함수를 호출합니다.
        if (!collision.gameObject.CompareTag("Player") || _isPrefab || _isBreaking) return;
        Execute();
    }

    /// <summary>
    /// 본인의 오브젝트를 복사하여 중력을 활성화 하여 떨어뜨리고, 변수 respawnTime이후에 다시 활성화 된다.
    /// </summary>
    private void Fall()
    {
        _isBreaking = true;
        GameManager.instance.AimToFallForced();
        
        //오브젝트 복사 후 SetPrefabMode함수 호출
        Instantiate(_groundThatWillFall, transform.position, Quaternion.identity)
            .GetComponent<FallingGround>().SetPrefabMode();
        
        gameObject.SetActive(false);
        Invoke(nameof(Respawn), respawnTime);
    }

    private void Respawn()
    {
        gameObject.SetActive(true);
        _isBreaking = false;
    }

    private bool _isPrefab = false;
    public float destroyPrefabTime;
    /// <summary>
    /// Fall 함수에서 복사하여 떨어지는 오브젝트에 적용된다.
    /// 중력을 적용하고, prefab임을 구분할수 있는 bool값을 true로 바꿔준다.
    /// 이 함수가 적용된 오브젝트는 destroyPrefabTime 이후에 삭제된다.
    /// </summary>
    private void SetPrefabMode()
    {
        _isPrefab = true;
        _rigid.isKinematic = false;
        GetComponent<Collider>().isTrigger = false;

        //invoke 함수는 input을 받을 수 없어 DeletePrefab 함수를 따로 만들었다.
        Invoke(nameof(DeletePrefab), destroyPrefabTime);
        
        SetRandomRotation(randomRotationSpeed);
    }
    private void DeletePrefab() => Destroy(gameObject);

    
    
    /// <summary>
    /// 오브젝트가 떨어질 때 호출한다. 랜덤방향으로 오브젝트가 회전한다.
    /// </summary>
    /// <param name="speed">오브젝트가 회전하는 속도</param>
    private void SetRandomRotation(float speed)
    {
        _rigid.angularVelocity = new Vector3
            (Random.value * randomRotationSpeed, 
                Random.value * randomRotationSpeed,
                Random.value * randomRotationSpeed);
    }
}
