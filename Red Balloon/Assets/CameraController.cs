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
    [SerializeField] private float dpi;

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
        if (onControll == ControllType.LookAround) LookAround();
        else if (onControll == ControllType.Drag && Input.GetMouseButton(1)) LookAround();
    }


    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseDelta *= dpi;
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


}
