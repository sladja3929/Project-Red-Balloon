using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Wind : Gimmick
{
    public float windPower;
    
    private AudioSource _windSound;
    private ParticleSystem[] _windEffects;

    private void OnTriggerStay(Collider other)
    {
        if (!isGimmickEnable) return;
        
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody>().AddForce(windPower * Time.deltaTime * -transform.right);
        }
    }
    
    private void Awake()
    {
        _windSound = GetComponent<AudioSource>();
        _windEffects = GetComponentsInChildren<ParticleSystem>();
    }

    private void Update()
    {
        foreach (var windEffect in _windEffects)
        {
            windEffect.gameObject.SetActive(isGimmickEnable);
        }
    }
}
