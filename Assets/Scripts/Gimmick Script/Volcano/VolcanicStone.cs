using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanicStone : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public AudioClip sound;
    public GameObject explosion;

    private bool _hasExploded;
    private void Awake()
    {
        _rigidbody = gameObject.AddComponent<Rigidbody>();

        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _hasExploded = false;
    }

    public void Fall(Vector3 direction, float speed)
    {
        _rigidbody.velocity = direction * speed;
        Transform transform1;
        (transform1 = transform).LookAt(transform.position + direction);
    }
    
    private void Update()
    {
#if UNITY_EDITOR
        if (_rigidbody is null) return;
        Debug.DrawRay(transform.position, _rigidbody.velocity * 100, Color.red);
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasExploded) return;

        _hasExploded = true;
        SoundManager.instance.SfxPlay("Volcanic stone sound", sound, transform.position);
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
