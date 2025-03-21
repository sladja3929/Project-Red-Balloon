using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SetSignPos : MonoBehaviour
{
    public static SetSignPos instance;

    private Rigidbody _rigidbody;
    private Vector3 pos;
    private Quaternion rot;
    private bool isOn = false;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        _rigidbody = GetComponent<Rigidbody>();
        GameManager.instance.onBalloonDead.AddListener(StandSign);
    }

    public void UpdateSignPosition(Transform point)
    {
        isOn = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        transform.SetPositionAndRotation(point.position, point.rotation);
        GameManager.instance.CanDie = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isOn && other.gameObject.CompareTag("Player"))
        {
            _rigidbody.constraints = RigidbodyConstraints.None;
        }
        
        else if (!isOn && other.gameObject.layer.Equals(3))
        {
            pos = transform.position;
            rot = transform.rotation;
            isOn = true;
            GameManager.instance.CanDie = true;
        }
    }

    private void StandSign()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        transform.position = pos;
        transform.rotation = rot;
    }

    private void OnDestroy()
    {
        GameManager.instance.onBalloonDead.RemoveListener(StandSign);
    }
}


