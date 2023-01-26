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

    public KeyCode dieKey;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _controller = GetComponent<BalloonController>();
    }

    private void Start()
    {
        //임시 세이브
        SetSavePoint(transform.position);
    }

    private void Update()
    {
        if (Input.GetKeyDown(dieKey))
        {
            Die();
        }
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
        var effect = Instantiate(dieEffect, transform1.position, Quaternion.identity);
        
        Destroy(effect, respawnTime);
        Invoke(nameof(Spawn), respawnTime);
    }

    private void Spawn()
    {
        transform.position = savePoint;
        _rigidbody.position = savePoint;
        _rigidbody.velocity = Vector3.zero;
        
        gameObject.SetActive(true);
        _controller.SetBasicState();
    }
}
