using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Vector3 savePoint;
    [SerializeField] private GameObject dieEffect;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private AudioClip dieSound;
    [SerializeField] private float respawnTime;

    private bool isSavePointReached;
    private int signPosIndex;

    private Rigidbody _rigidbody;
    private BalloonController _controller;
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;

    private SetSignPos setSignPos;
    private SetSaveByCollision setSaveByCollision;

    public KeyCode dieKey;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _controller = GetComponent<BalloonController>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
        
        setSignPos = FindObjectOfType<SetSignPos>();
        setSaveByCollision = FindObjectOfType<SetSaveByCollision>();
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
    public void SetSavePointReached(bool isReached)
    {
        isSavePointReached = isReached;
    }

    public void SetSignPosIndex(int index)
    {
        signPosIndex = index;
    }

    public void Die()
    {
        //폭발 이펙트를 남기고 죽음
        //n초후 저장된 리스폰 포인트에 부활함
        //부활할때 특정 이펙트나 연출이 있을 수 있으니 부활은 함수로 처리

        _meshCollider.enabled = false;
        _meshRenderer.enabled = false;
        _rigidbody.useGravity = false;
        _controller.SetFreezeState();
        
        GameManager.instance.BalloonDeadEvent();
        
        var transform1 = transform;
        var effect = Instantiate(dieEffect, transform1.position, transform1.rotation);
        
        SoundManager.instance.SfxPlay("BalloonPop", dieSound, transform.position);
        
        Destroy(effect, respawnTime);
        Invoke(nameof(Spawn), respawnTime);

    }
    
    private void Spawn()
    {
        deathUI.SetActive(true);

        transform.position = savePoint;
        transform.rotation = Quaternion.Euler(180, 0, 0);
        _meshRenderer.enabled = true;
        
        //_rigidbody.position = savePoint;
        //_rigidbody.velocity = Vector3.zero;
        
        StartCoroutine(SpawnCoroutine());

        if (isSavePointReached && setSignPos != null)
        {
            setSignPos.UpdateSignPosition(signPosIndex);
            isSavePointReached = false;
        }
    }

    private IEnumerator SpawnCoroutine()
    {
        var curScale = Vector3.one;
        float yPos = savePoint.y;
        
        for (int i = 0; i < 100; i++)
        {
            transform.localScale = curScale * (0.01f * i) ;
            transform.position = new Vector3(savePoint.x, yPos -= 0.01f, savePoint.z);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localScale = new Vector3(1, 1, 1);

        _meshCollider.enabled = true;
        _rigidbody.useGravity = true;

        _controller.SetBasicState();
    }
}
