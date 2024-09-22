 using System;
 using System.Collections;
using System.Collections.Generic;
 using Unity.Mathematics;
 using UnityEngine;
 using UnityEngine.Rendering;
 using UnityEngine.Rendering.Universal;

 public class Stage2Event : MonoBehaviour
{
    [SerializeField] private GameObject sun;
    [SerializeField] private VolumeProfile volume;
    [SerializeField] private float fadeTime;
    [SerializeField] private Vector3 shadMidRGB = new Vector3(1, 0.65f, 0.65f);
    [SerializeField] ParticleSystem volcanoSmoke;
    [SerializeField] private bool debug;
    
    private ShadowsMidtonesHighlights _shadMid;
    private Vector4 initValue;
    private bool isChange = false;
    
    // Start is called before the first frame update
    void Start()
    {
        volcanoSmoke.Stop();
        volume.TryGet<ShadowsMidtonesHighlights>(out _shadMid);
        initValue = _shadMid.shadows.value;
        if (debug) SunSet();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isChange)
        {
            SunSet();
        }
    }

    private void SunSet()
    {
        isChange = true;
        volcanoSmoke.Play();
        _shadMid.shadows.overrideState = true;
        _shadMid.midtones.overrideState = true;
        StartCoroutine(SunSetCoroutine(initValue));
    }
    
    private IEnumerator SunSetCoroutine(Vector4 value)
    {
        float time = 0;
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            sun.GetComponent<Light>().intensity = Mathf.Lerp(1, 0.1f, time / fadeTime);
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
