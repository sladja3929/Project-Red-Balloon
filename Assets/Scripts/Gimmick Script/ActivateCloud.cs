using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ActivateCloud : MonoBehaviour
{
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
    private MeshRenderer _meshRenderer;
    private int countTrigger;
    private bool isIn;
    
    [SerializeField] private atmosphereAtttribute sunny = new atmosphereAtttribute(
        new Color(58,207,224), new Color(59, 166, 233), 0.0005f, 0.5f, 5000f);

    [SerializeField] private atmosphereAtttribute cloud = new atmosphereAtttribute(
        new Color(229,229,229), new Color(229,229,229), 0.05f, 0.1f, 30f);

    [SerializeField] private float duration = 1;
    [SerializeField] private float smoothness = 0.05f;
    
    // Start is called before the first frame update
    private void Start()
    {
        skybox = RenderSettings.skybox;
        virtualCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
        originalFarPlane = virtualCamera.m_Lens.FarClipPlane;
        sunny.cameraFarPlane = originalFarPlane;
        _meshRenderer = transform.GetComponent<MeshRenderer>();
        InitSettings(sunny);
        countTrigger = 0;
        isIn = false;
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ++countTrigger;
            
            if (!isIn)
            {
                isIn = true;
                SetFarPlane(30f);
                _meshRenderer.enabled = false;
                StartCoroutine(ChangeAtmosphere(sunny, cloud));
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
                StartCoroutine(ChangeAtmosphere(cloud, sunny));
            }
        }
    }

    private void OnDestroy()
    {
        InitSettings(sunny);
    }
}