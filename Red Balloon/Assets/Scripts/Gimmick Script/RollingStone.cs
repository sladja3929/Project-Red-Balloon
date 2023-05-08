using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RollingStone : Gimmick
{
    private Rigidbody _rigidbody;
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
    }
    
    public override void Execute()
    {
        _rigidbody.isKinematic = false;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player")) GameManager.Instance.KillBalloon();
        if (other.collider.CompareTag($"Rolling Stone Killer")) Invoke(nameof(DelObject), delCooldown);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) GameManager.Instance.KillBalloon();
        if (other.CompareTag($"Rolling Stone Killer")) Invoke(nameof(DelObject), delCooldown);
    }
    
    public float delCooldown;
    private void DelObject()
    {
        Destroy(gameObject);
    }
}
