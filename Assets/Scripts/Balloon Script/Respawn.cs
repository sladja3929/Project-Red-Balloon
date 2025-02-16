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
    [SerializeField] private KeyCode skipKey;
    
    private bool isSavePointReached;
    private Vector3 respawnAngle;

    private Rigidbody _rigidbody;
    private BalloonController _controller;
    private MeshRenderer[] _meshRenderers;
    private MeshCollider _meshCollider;

    private SetSignPos setSignPos;
    private SetSaveByCollision setSaveByCollision;

    public KeyCode dieKey;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _controller = GetComponent<BalloonController>();
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
        
        setSignPos = FindObjectOfType<SetSignPos>();
        setSaveByCollision = FindObjectOfType<SetSaveByCollision>();
    }

    private void Start()
    {
        //임시 세이브
        SetRespawnPoint(transform.position);

    }

    private void Update()
    {
        if (Input.GetKeyDown(dieKey))
        {
            GameManager.instance.KillBalloon();
        }
    }

    public void SetRespawnPoint(Vector3 point)
    {
        savePoint = point;
    }

    public void SetRespawnAngle(Vector3 angle)
    {
        respawnAngle = angle;
    }

    public Quaternion GetRespawnAngle()
    {
        return Quaternion.Euler(respawnAngle);
    }
    
    public void SetSavePointReached(bool isReached)//세이브포인트 도달 여부 설정, 이후 스폰 시 사용
    {
        isSavePointReached = isReached;
    }

    public void Die()
    {
        //폭발 이펙트를 남기고 죽음
        //n초후 저장된 리스폰 포인트에 부활함
        //부활할때 특정 이펙트나 연출이 있을 수 있으니 부활은 함수로 처리

        GameManager.instance.CanDie = false;
        GameManager.instance.AimToFallForced();
        _meshCollider.enabled = false;
        foreach (var _meshRenderer in _meshRenderers)
            _meshRenderer.enabled = false;
        _rigidbody.useGravity = false;
        _controller.SetFreezeState();
        
        SaveManager.instance.DeathCount++;
        SaveManager.instance.Save();
        GameManager.instance.BalloonDeadEvent();
        
        var transform1 = transform;
        var effect = Instantiate(dieEffect, transform1.position, transform1.rotation);
        
        SoundManager.instance.SfxPlay("BalloonPop", dieSound, transform.position);
        
        Destroy(effect, respawnTime);
        Invoke(nameof(Spawn), respawnTime);
    }
    
    private void Spawn()
    {
        GameManager.instance.BalloonRespawnEvent();
        
        transform.position = savePoint;
        transform.rotation = Quaternion.Euler(180, 0, 0);
        CameraController.instance.SetRotation(respawnAngle);
        foreach (var _meshRenderer in _meshRenderers)
            _meshRenderer.enabled = true;
        
        //_rigidbody.position = savePoint;
        //_rigidbody.velocity = Vector3.zero;
        
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        var curScale = Vector3.one;
        float yPos = savePoint.y;
        
        for (int i = 0; i < 50; i++)
        {
            //if (Input.GetKeyDown(skipKey)) break;
            transform.localScale = curScale * (0.02f * i) ;
            transform.position = new Vector3(savePoint.x, yPos -= 0.02f, savePoint.z);
            yield return new WaitForSeconds(0.01f);
        }

        transform.position = new Vector3(savePoint.x, savePoint.y - 1f, savePoint.z);
        transform.localScale = new Vector3(1, 1, 1);

        _meshCollider.enabled = true;
        _rigidbody.useGravity = true;
        _controller.SetBasicState();
        GameManager.instance.CanDie = true;
    }
}
