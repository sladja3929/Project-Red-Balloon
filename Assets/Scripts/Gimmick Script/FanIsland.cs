using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FanIsland : Gimmick
{
    [SerializeField]private float rotateTime;
    [SerializeField]private float maxSpeedPoint;
    [SerializeField]private float coolDown;
    [SerializeField]private float maxRotateSpeed;
    
    private Vector3 rotation;
    private Wind windScript;
    private bool isOn;
    private bool isCoolDown;
    private float time = 0;
    
    private void Awake()
    {
        rotation = transform.localRotation.eulerAngles;
        windScript = transform.parent.GetComponentInChildren<Wind>();
        isOn = false;
        isCoolDown = false;
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

    private IEnumerator FlyCoroutine()
    {
        while (true)
        {
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

    private void FixedUpdate()
    {
        if (isGimmickEnable)
        {
            time += Time.fixedDeltaTime;
            
            if (!isCoolDown)
            {
                if(isOn) GameManager.instance.AimToFallForced();

                if (time < rotateTime)
                {
                    float percentage = Mathf.Min(time / (maxSpeedPoint * rotateTime), 1f) * Mathf.Min((rotateTime - time) / (maxSpeedPoint * rotateTime), 1f);
                    
                    rotation.y += percentage * maxRotateSpeed;
                    transform.localEulerAngles = rotation;
                
                    if(percentage > 0.6f) windScript.GimmickOn();
                    else windScript.GimmickOff();
                }

                else
                {
                    isCoolDown = true;
                    time = 0;
                }
            }

            else
            {
                if (time > coolDown)
                {
                    isCoolDown = false;
                    time = 0;
                }
            }
        }
    }
}
