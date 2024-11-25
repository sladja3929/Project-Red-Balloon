using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private Transform viewerTransform;
    private static float DPI => StaticSensitivity.camSensitivity;

    public enum ControllType
    {
        Drag,
        LookAround,
        Stop
    }

    public ControllType onControll = ControllType.Stop;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.IsPause) return;
        
        if (Input.GetMouseButton(1))
        {
            if (onControll is ControllType.Drag) LookAround(); 
        }
        
        if(onControll is ControllType.LookAround) LookAround();
    }


    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseDelta *= DPI;
        Vector3 camAngle = viewerTransform.rotation.eulerAngles;

        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        viewerTransform.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }

    public Quaternion GetRotation()
    {
        return viewerTransform.rotation;
    }
    
    public void SetDpiRate(float rate)
    {
        dpi = Mathf.Lerp(0.1f, 10, rate);
    }
    
    public float GetDpiRate()
    {
        return (dpi - 0.1f) / (10 - 0.1f);
    }
}
