 using System;
 using System.Collections;
using System.Collections.Generic;
 using Unity.Mathematics;
 using UnityEngine;
 using UnityEngine.Rendering;
 using UnityEngine.Rendering.Universal;

 public class Stage2Event : MonoBehaviour
{
    [SerializeField] private Light sun;
    [SerializeField] private VolumeProfile volume;
    [SerializeField] private float fadeTime;
    [SerializeField] private Vector3 shadMidRGB = new Vector3(1, 0.65f, 0.65f);
    [SerializeField] private ParticleSystem volcanoEffect;
    
    private ShadowsMidtonesHighlights _shadMid;
    private Vector4 initValue;
    private bool isChange = false;
    
    // Start is called before the first frame update
    void Start()
    {
        volume.TryGet<ShadowsMidtonesHighlights>(out _shadMid);
        InitSettings();
    }

    private void InitSettings()
    {
        initValue = _shadMid.shadows.value;
        volcanoEffect.Stop();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isChange)
        {
            StartCoroutine(SunSetEvent(initValue));
        }
    }

    private void SunSetForced()
    {
        isChange = true;
        volcanoEffect.Play();
        _shadMid.shadows.overrideState = true;
        _shadMid.midtones.overrideState = true;
        sun.intensity = 0.1f;
        
        Vector4 value = initValue;
        value.y = shadMidRGB.y;
        value.z = shadMidRGB.z;
        _shadMid.shadows.SetValue(new UnityEngine.Rendering.Vector4Parameter(value));
        _shadMid.midtones.SetValue(new UnityEngine.Rendering.Vector4Parameter(value));
    }
    
    private IEnumerator SunSetEvent(Vector4 value)
    {
        isChange = true;
        volcanoEffect.Play();
        _shadMid.shadows.overrideState = true;
        _shadMid.midtones.overrideState = true;
        
        float time = 0;
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            sun.intensity = Mathf.Lerp(1, 0.1f, time / fadeTime);
            value.y = Mathf.Lerp(initValue.y, shadMidRGB.y, time / fadeTime);
            value.z = Mathf.Lerp(initValue.z, shadMidRGB.z, time / fadeTime);
            _shadMid.shadows.SetValue(new UnityEngine.Rendering.Vector4Parameter(value));
            _shadMid.midtones.SetValue(new UnityEngine.Rendering.Vector4Parameter(value));
            yield return null;
        }
    }

    private void OnDestroy()
    {
        _shadMid.shadows.overrideState = false;
        _shadMid.midtones.overrideState = false;
        _shadMid.shadows.SetValue(new UnityEngine.Rendering.Vector4Parameter(initValue));
        _shadMid.midtones.SetValue(new UnityEngine.Rendering.Vector4Parameter(initValue));
    }
}
