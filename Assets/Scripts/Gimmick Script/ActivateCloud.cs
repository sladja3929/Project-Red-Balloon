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
        countTrigger = 0;
        isIn = false;
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
                CloudEvent.Instance.EnterCloud();
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
                isIn = false;
                _meshRenderer.enabled = true;
                CloudEvent.Instance.ExitCloud();
            }
        }
    }
}