using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveEndEvent : MonoBehaviour
{
    [SerializeField] private GameObject ambientLight;
    [SerializeField] private float intensity;
    [SerializeField] private float fadeTime;

    private Light light;
    private float baseIntensity;
    private void Start()
    {
        light = ambientLight.GetComponent<Light>();
        baseIntensity = light.intensity;
        ambientLight.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !ambientLight.activeSelf)
        {
            StartCoroutine(SetAmbientLightEvent());
        }
    }
    
    private IEnumerator SetAmbientLightEvent()
    {
        ambientLight.SetActive(true);
        
        float time = 0;
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            light.intensity = Mathf.Lerp(baseIntensity, intensity, time / fadeTime);
            yield return null;
        }
    }

    private void SetAmbientLightForced()
    {
        ambientLight.SetActive(true);
        light.intensity = intensity;
    }
}
