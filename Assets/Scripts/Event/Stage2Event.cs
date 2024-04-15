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
    [SerializeField] private bool debug;
    private ShadowsMidtonesHighlights shadMid;
    private Vector4 initValue;
    private bool isChange = false;
    
    // Start is called before the first frame update
    void Start()
    {
        volume.TryGet<ShadowsMidtonesHighlights>(out shadMid);
        initValue = shadMid.shadows.value;
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
        shadMid.shadows.overrideState = true;
        shadMid.midtones.overrideState = true;
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
            shadMid.shadows.SetValue(new UnityEngine.Rendering.Vector4Parameter(value));
            shadMid.midtones.SetValue(new UnityEngine.Rendering.Vector4Parameter(value));
            yield return null;
        }
    }

    private void OnDestroy()
    {
        shadMid.shadows.overrideState = false;
        shadMid.midtones.overrideState = false;
        shadMid.shadows.SetValue(new UnityEngine.Rendering.Vector4Parameter(initValue));
        shadMid.midtones.SetValue(new UnityEngine.Rendering.Vector4Parameter(initValue));
    }
}
