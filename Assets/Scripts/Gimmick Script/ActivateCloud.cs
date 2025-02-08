using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ActivateCloud : Gimmick
{
    private MeshRenderer _meshRenderer;
    private int countTrigger;
    private bool isIn;
    
    private void Start()
    {
        _meshRenderer = transform.GetComponent<MeshRenderer>();
        InitSettings();
        GameManager.instance.onBalloonRespawn.AddListener(InitSettings);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isGimmickEnable && other.CompareTag("Player"))
        {
            ++countTrigger;
            
            if (!isIn)
            {
                isIn = true;
                _meshRenderer.enabled = false;
                CloudEvent.instance.EnterCloud();
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            --countTrigger;
            if (countTrigger < 0) countTrigger = 0;

            if (countTrigger == 0 && isIn)
            {
                InitSettings();
                CloudEvent.instance.ExitCloud();
            }
        }
    }

    private void InitSettings()
    {
        countTrigger = 0;
        isIn = false;
        _meshRenderer.enabled = true;
    }

    private void OnDestroy()
    {
        GameManager.instance.onBalloonRespawn.RemoveListener(InitSettings);
    }
}