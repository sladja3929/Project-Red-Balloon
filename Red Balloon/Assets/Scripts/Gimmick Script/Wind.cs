using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Wind : Gimmick
{
    public Vector3 windDirection;
    public float windPower;

    private void OnTriggerStay(Collider other)
    {
        if (!isGimmickEnable) return;
        
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody>().AddForce(windPower * Time.deltaTime * windDirection.normalized);
        }
    }
    

    private AudioSource windSound;
    private void Awake()
    {
        windSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        windSound.volume = SoundManager.Instance.GetSfxSoundVolume();
    }
}
