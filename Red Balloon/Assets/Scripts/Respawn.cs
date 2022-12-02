using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Vector3 savePoint;
    [SerializeField] private GameObject dieEffect;
    [SerializeField] private float respawnTime;

    private Rigidbody _rigidbody;
    private BalloonController _controller;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _controller = GetComponent<BalloonController>();
    }

    public void SetSavePoint(Vector3 point)
    {
        savePoint = point;
    }

    public void Die()
    {
        //폭발 이펙트를 남기고 죽음
        //n초후 저장된 리스폰 포인트에 부활함
        //부활할때 특정 이펙트나 연출이 있을 수 있으니 부활은 함수로 처리
        gameObject.SetActive(false);
        
        var transform1 = transform;
        Instantiate(dieEffect, transform1.position, transform1.rotation);
        
        
        
        Invoke(nameof(Spawn), respawnTime);
    }

    private void Spawn()
    {
        _rigidbody.position = savePoint;
        _rigidbody.velocity = Vector3.zero;
        _controller.SetBasicState();
        gameObject.SetActive(true);
    }
}
