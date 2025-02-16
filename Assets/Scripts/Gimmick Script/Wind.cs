using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Wind : Gimmick
{
    [SerializeField] private float windPower;
    
    private AudioSource _windSound;
    private ParticleSystem[] _windEffects;
    private Rigidbody playerRb;
    private bool playerInside = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInside = true;
            playerRb = other.GetComponent<Rigidbody>();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            BalloonExit();
        }
    }

    private void BalloonExit()
    {
        playerInside = false;
        playerRb = null;    
    }
    
    private void FixedUpdate()
    {
        foreach (var windEffect in _windEffects)
        {
            windEffect.gameObject.SetActive(isGimmickEnable);
        }
        
        if (isGimmickEnable && playerInside && playerRb != null)
        {
            // FixedUpdate에서는 Time.fixedDeltaTime 사용
            playerRb.AddForce(windPower * Time.fixedDeltaTime * -transform.right, ForceMode.Force);
        }
    }
    
    private void Awake()
    {
        _windSound = GetComponent<AudioSource>();
        _windEffects = GetComponentsInChildren<ParticleSystem>();
        foreach (var windEffect in _windEffects)
        {
            windEffect.gameObject.SetActive(isGimmickEnable);
        }
    }

    private void Start()
    {
        GameManager.instance.onBalloonDead.AddListener(BalloonExit);
    }

    private void OnDestroy()
    {
        GameManager.instance.onBalloonDead.RemoveListener(BalloonExit);
    }
}
