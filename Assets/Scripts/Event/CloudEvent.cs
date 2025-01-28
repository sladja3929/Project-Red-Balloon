using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CloudEvent : MonoBehaviour
{
    public static CloudEvent Instance;
    [Serializable] private struct atmosphereAtttribute
    {
        public Color skyColor;
        public Color seaColor;
        public float fogDensity;
        public float sunHaloContribution;
        public float cameraFarPlane;
        
        public atmosphereAtttribute(Color skyColor, Color seaColor, float fogDensity, float sunHaloContribution, float cameraFarPlane)
        {
            this.skyColor = skyColor;
            this.seaColor = seaColor;
            this.sunHaloContribution = sunHaloContribution;
            this.fogDensity = fogDensity;
            this.cameraFarPlane = cameraFarPlane;
        }
    }
    
    private Material skybox;
    private CinemachineVirtualCamera virtualCamera;
    private float originalFarPlane;
    
    [SerializeField] private atmosphereAtttribute sunny = new atmosphereAtttribute(
        new Color(14,180,252), new Color(195,208,217), 0.003f, 0.5f, 5000f);

    [SerializeField] private atmosphereAtttribute cloud = new atmosphereAtttribute(
        new Color(255,255,255), new Color(255,255,255), 0.055f, 0.1f, 100f);

    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float smoothness = 0.05f;

    private void Start()
    {
        skybox = RenderSettings.skybox;
        virtualCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
        originalFarPlane = virtualCamera.m_Lens.FarClipPlane;
        sunny.cameraFarPlane = originalFarPlane;
        InitSettings(sunny);
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void InitSettings(atmosphereAtttribute attr)
    {
        skybox.SetColor("_SkyGradientTop", attr.skyColor);
        skybox.SetColor("_SkyGradientBottom", attr.seaColor);
        skybox.SetFloat("_SunHaloContribution", attr.sunHaloContribution);
        
        RenderSettings.fogDensity = attr.fogDensity;
        RenderSettings.fogColor = cloud.skyColor;
        
        SetFarPlane(attr.cameraFarPlane);
    }

    private void SetFarPlane(float value)
    {
        if (virtualCamera == null) return;
        
        Cinemachine.LensSettings lens = virtualCamera.m_Lens;
        lens.FarClipPlane = value;
        virtualCamera.m_Lens = lens;
    }
    
    private IEnumerator ChangeAtmosphere(atmosphereAtttribute start, atmosphereAtttribute end)
    {
        float progress = 0;
        float increment = smoothness / duration;
        float currentFloat;
        Color currentColor;
        
        
        
        while (progress < 1)
        {
            currentColor = Color.Lerp(end.skyColor * 0.9f, end.skyColor, progress);
            skybox.SetColor("_SkyGradientTop", currentColor);
            //RenderSettings.fogColor = currentColor;
            
            currentColor = Color.Lerp(end.seaColor * 0.9f, end.seaColor, progress);
            skybox.SetColor("_SkyGradientBottom", currentColor);
            
            currentFloat = Mathf.Lerp(start.sunHaloContribution, end.sunHaloContribution, progress);
            skybox.SetFloat("_SunHaloContribution", currentFloat);
            
            currentFloat = Mathf.Lerp(start.fogDensity, end.fogDensity, progress);
            RenderSettings.fogDensity = currentFloat;
            
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }

        InitSettings(end);
    }
    
    private void OnDestroy()
    {
        InitSettings(sunny);
        SetFarPlane(originalFarPlane);
    }
    
    public void EnterCloud()
    {
        StartCoroutine(ChangeAtmosphere(sunny, cloud));
    }
    
    public void ExitCloud()
    {
        StartCoroutine(ChangeAtmosphere(cloud, sunny));
    }
}