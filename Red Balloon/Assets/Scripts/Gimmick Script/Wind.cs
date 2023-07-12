using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Wind : Gimmick
{
    public Vector3 windDirection;
    public float windPower;
    
    private AudioSource _windSound;
    private ParticleSystem _windEffect;

    private void OnTriggerStay(Collider other)
    {
        if (!isGimmickEnable) return;
        
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody>().AddForce(windPower * Time.deltaTime * windDirection.normalized);
        }
    }
    
    private void Awake()
    {
        _windSound = GetComponent<AudioSource>();
        _windEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        _windSound.volume = SoundManager.instance.GetSfxSoundVolume() + 0.5f;
        if (_windSound.volume > 1) _windSound.volume = 1;
        _windEffect.gameObject.SetActive(isGimmickEnable);
    }
}
