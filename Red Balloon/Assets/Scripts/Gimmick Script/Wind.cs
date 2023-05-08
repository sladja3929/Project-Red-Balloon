using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Wind : Gimmick
{
    public Vector3 windDirection;
    public float windPower;
    public GameObject windEffect;

    private void Awake()
    {
        windEffect = GetComponentInChildren<ParticleSystem>().gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isGimmickEnable) return;
        
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody>().AddForce(windPower * Time.deltaTime * windDirection.normalized);
        }
    }

    private void Update()
    {
        windEffect.SetActive(isGimmickEnable);
    }
}
