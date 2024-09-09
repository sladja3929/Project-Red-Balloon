using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Vector3 savePoint;
    [SerializeField] private GameObject dieEffect;
    [SerializeField] private AudioClip dieSound;
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
        var effect = Instantiate(dieEffect, transform1.position, transform1.rotation);
        
        AudioSource.PlayClipAtPoint(dieSound, transform.position);
        
        Destroy(effect, respawnTime);
        Invoke(nameof(Spawn), respawnTime);
    }
    
    private void Spawn()
    {
        transform.position = savePoint;
        transform.rotation = quaternion.Euler(90, 0, 0);
        _rigidbody.position = savePoint;
        _rigidbody.velocity = Vector3.zero;

        gameObject.SetActive(true);
        _controller.SetBasicState();

        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        _rigidbody.isKinematic = true;
        var curScale = Vector3.one;
        for (int i = 0; i < 100; i++)
        {
            transform.localScale = curScale * (1 * i) ;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.01f, transform.position.z);
            yield return new WaitForSeconds(0.01f);
        }

        transform.localScale = new Vector3(100, 100, 100);
        _rigidbody.isKinematic = false;
    }
}
