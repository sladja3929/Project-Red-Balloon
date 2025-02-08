using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanIsland : Gimmick
{
    [SerializeField]private float rotateTime;
    [SerializeField]private float maxSpeedPoint;
    [SerializeField]private float coolDown;
    [SerializeField]private float maxRotateSpeed;
    [SerializeField] private float delay;
    
    private Vector3 rotation;
    private Wind windScript;
    private bool isOn;
    private void Awake()
    {
        rotation = transform.localRotation.eulerAngles;
        windScript = transform.parent.GetComponentInChildren<Wind>();
        isOn = false;
    }
    private void Start()
    {
        Execute();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isOn = true;
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isOn = false;
        }
    }

    [ContextMenu("Execute")]    
    public override void Execute()
    {
        if (isGimmickEnable is false) return;

        StartCoroutine(FlyCoroutine());
    }

    private IEnumerator FlyCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            
            if(isOn) GameManager.instance.AimToFallForced();
            
            float t = 0;
            while ((t += Time.deltaTime) < rotateTime)
            {
                float percentage = Mathf.Min(t / (maxSpeedPoint * rotateTime), 1f) * Mathf.Min((rotateTime - t) / (maxSpeedPoint * rotateTime), 1f);

                rotation.y += percentage * maxRotateSpeed;
                transform.localEulerAngles = rotation;
                
                if(percentage > 0.6f) windScript.GimmickOn();
                else windScript.GimmickOff();
                
                yield return null;
            }
            
            yield return new WaitForSeconds(coolDown);
        }
    }
}
