using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class RollingStone : Gimmick
{
    [SerializeField] private float deleteTime;
    [SerializeField] private float resetTime;
    
    private Rigidbody _rigidbody;
    private MeshCollider _meshCollider;
    private MeshRenderer _meshRenderer;
    private AudioSource _audio;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private bool isPlayed = false;
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _meshCollider = GetComponent<MeshCollider>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _rigidbody.isKinematic = true;
        _audio = GetComponent<AudioSource>();
        originalPos = transform.position;
        originalRot = transform.rotation;
    }
    
    public override void Execute()
    {
        if (!isPlayed)
        {
            isPlayed = true;
            _rigidbody.isKinematic = false;
            _audio.Play();
        }
    }

    private void InitStone()
    {
        isPlayed = false;
        _meshCollider.enabled = true;
        _meshRenderer.enabled = true;
        transform.SetPositionAndRotation(originalPos, originalRot);
        _rigidbody.isKinematic = true;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player")) GameManager.instance.KillBalloon();
        if (other.collider.CompareTag($"Rolling Stone Killer")) Invoke(nameof(ResetStone), deleteTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) GameManager.instance.KillBalloon();
        if (other.CompareTag($"Rolling Stone Killer")) Invoke(nameof(ResetStone), deleteTime);
    }
    
    private void ResetStone()
    {
        _meshCollider.enabled = false;
        _meshRenderer.enabled = false;
        Invoke(nameof(InitStone), resetTime);
    }
}
