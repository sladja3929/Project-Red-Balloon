using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanIsland : Gimmick
{
    [SerializeField]private float rotateTime;
    [SerializeField]private float coolDown;
    [SerializeField]private float maxRotateSpeed;

    private Vector3 rotation;
    private Wind windScript;
    private bool isOn;
    private void Awake()
    {
        rotation = transform.rotation.eulerAngles;
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
            if(isOn) GameManager.instance.AimToFallForced();
            windScript.GimmickOn();
            float t = 0;
            while ((t += Time.deltaTime) < rotateTime)
            {
                float percentage = 1 - 2 * Mathf.Abs((t / rotateTime) - 0.5f);

                rotation.y += percentage * maxRotateSpeed;
                transform.eulerAngles = rotation;
                
                yield return null;
            }
            windScript.GimmickOff();
            
            yield return new WaitForSeconds(coolDown);
        }
    }
}
