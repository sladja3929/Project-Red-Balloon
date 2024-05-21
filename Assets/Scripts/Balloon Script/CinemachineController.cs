using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    public static CinemachineController instance;
    
    private CinemachineFreeLook freeLook;

    public enum ControllType
    {
        Drag,
        LookAround,
        Stop
    }
    
    public ControllType onControll = ControllType.Stop;
    
    void Awake()
    {
        instance = this;
        CinemachineCore.GetInputAxis = ClickControl;
    }

    private void FixedUpdate()
    {
        if (GameManager.isPause) return;
        
    }

    void Start()
    {
        InitSetting();
    }

    private void InitSetting()
    {
        // Target 설정
        freeLook = this.GetComponent<CinemachineFreeLook>();
        freeLook.Follow = GameObject.FindWithTag("Player").transform;
        freeLook.LookAt = GameObject.FindWithTag("Player").transform;

        // // Lens 설정
        // freeLook.m_Lens.FieldOfView = 120;
        // freeLook.m_Lens.NearClipPlane = 0.2f;
        // freeLook.m_Lens.FarClipPlane = 5000;
        // freeLook.m_Lens.Dutch = 0;
        // //freeLook.m_Lens.ModeOverride = LensSettings.OverrideModes.None;
        //
        // // Axis Control 설정
        // freeLook.m_XAxis.Value = 0;
        // freeLook.m_XAxis.m_MaxSpeed = 300;
        // freeLook.m_XAxis.m_AccelTime = 0.1f;
        //
        // // Rig 설정
        // CinemachineComposer composer
        //     = freeLook.GetRig(1).GetCinemachineComponent<CinemachineComposer>();
        // composer.m_DeadZoneHeight = 0.3f;
        // composer.m_DeadZoneWidth = 0.5f;
    }
    
    public float ClickControl(string axis)
    {
        if (Input.GetMouseButton(1))
            return UnityEngine.Input.GetAxis(axis);

        return 0;
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }
}