using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RotateIsland : Gimmick
{
    [SerializeField] private float[] rotateAngle = new float[7];
    [SerializeField] private float rotateDelay = 5f;
    [SerializeField] private float rotateTime = 3f;
    private Quaternion originalAngle;
    private Quaternion targetAngle;
    private float timer;
    private short step;
    private short dir;

    private bool activate;

    private bool onPlatform;
    // Start is called before the first frame update
    private void Start()
    {
        activate = false;
        onPlatform = false;
        timer = 0;
        step = 3;
        originalAngle = transform.localRotation;
        targetAngle = originalAngle;
    }

    private void FixedUpdate()
    {
        if (isGimmickEnable && activate)
        {
            timer += Time.deltaTime;
            
            if (timer >= rotateDelay)
            {
                //transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetAngle, rotateSpeed * Time.deltaTime);
                activate = false;
                timer = 0;
                StartCoroutine("RotatePlatform");
            }
        }
    }

    private IEnumerator RotatePlatform()
    {
        if (SetTarget())
        {
            var startAngle = transform.localRotation;
            var elapsed = 0f;
            if(onPlatform) GameManager.instance.AimToFallForced();

            while (elapsed < rotateTime)
            {
                transform.rotation = Quaternion.Slerp(startAngle, targetAngle, elapsed / rotateTime);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetAngle;
            if(onPlatform) GameManager.instance.FallToAimForced();

            if (dir != 0)
            {
                //activate = true;
                //SetTarget();
            }
        }
              
    }

    private bool SetTarget()
    {
        if (dir == 0)
        {
            if (step == 3) return false;
            step = 3;
        }
        else step += dir;
        targetAngle = originalAngle * Quaternion.Euler(0, 0, rotateAngle[step]);
        Debug.Log(step);
        return true;
    }
    
    public void EnterPlatform(short _dir)
    {
        if (!isGimmickEnable) return;

        activate = true;
        onPlatform = true;
        dir = _dir;
        timer = 0;
    }

    public void ExitPlatform()
    {
        if (!isGimmickEnable) return;

        activate = true;
        onPlatform = false;
        dir = 0;
        timer = 0;
    }
}
