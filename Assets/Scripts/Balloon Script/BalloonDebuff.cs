using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonDebuff : MonoBehaviour
{
    private Vector3 _originalScale;
    private float _gauge;
    
    [SerializeField]
    private float maxSize;
    
    private float heatingSpeed;
    
    private bool isHeated;

    void Start()
    {
        _originalScale = transform.localScale;
        GameManager.instance.onBalloonDead.AddListener(InitSettings);
        InitSettings();
    }

    private void InitSettings()
    {
        transform.localScale = _originalScale;
        heatingSpeed = 0;
        _gauge = 0;
        isHeated = false;
        this.enabled = false;
    }
    
    private void FixedUpdate()
    {
        transform.localScale = _originalScale * (1 + maxSize * _gauge);

        if (isHeated)
        {
            _gauge += heatingSpeed;
            
            if (_gauge > 1)
            {
                GameManager.instance.KillBalloon();
            }
        }

        else
        {
            _gauge -= heatingSpeed;
            
            if (_gauge < 0)
            {
                InitSettings();
            }
        }
    }

    public void HeatBalloon(float heatingPower)
    {
        heatingSpeed = heatingPower;
        isHeated = true;
    }

    public void ColdBalloon()
    {
        isHeated = false;
    }

    private void OnDestroy()
    {
        GameManager.instance.onBalloonDead.RemoveListener(InitSettings);
    }
}
