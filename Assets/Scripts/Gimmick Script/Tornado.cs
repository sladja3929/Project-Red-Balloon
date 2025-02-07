using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tornado : Gimmick
{
    [SerializeField] private float sizeMultiplier;
    [SerializeField] private float cycleTime;
    [SerializeField] private float delay;
    
    [SerializeField] private float pullPower = 5;
    [SerializeField] private float upPower = 2;
    [SerializeField] private float refreshRate = 1;
    [Range(0, 90)]
    [SerializeField] private float rotationDegree = 5;
    
    private Coroutine _tornado;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private AudioSource audio;
    private float originalMinDist;
    
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        originalMinDist = audio.minDistance;
        originalScale = transform.localScale;
        targetScale = new Vector3(originalScale.x * sizeMultiplier, originalScale.y, originalScale.z * sizeMultiplier);
        StartCoroutine(ChangeSize());
    }

    private IEnumerator ChangeSize()
    {
        float halfTime = cycleTime / 2f;
        float targetMinDist = originalMinDist * sizeMultiplier;
        
        while (true)
        {
            float elapsed = 0f;
            while (elapsed < halfTime)
            {
                float t = elapsed / halfTime;
                transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
                audio.minDistance = Mathf.Lerp(originalMinDist, targetMinDist, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            audio.minDistance = targetMinDist;
            transform.localScale = targetScale;

            yield return new WaitForSeconds(delay);

            elapsed = 0f;
            while (elapsed < halfTime)
            {
                float t = elapsed / halfTime;
                transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
                audio.minDistance = Mathf.Lerp(originalMinDist, targetMinDist, t);
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            audio.minDistance = originalMinDist;
            transform.localScale = originalScale;
            
            yield return new WaitForSeconds(delay);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && isGimmickEnable)
        {
            if (_tornado is not null) return;
            GameManager.instance.CanSuicide = false;
            GameManager.instance.AimToFallForced();
            _tornado = StartCoroutine(PullObject(col));
        }
    }
    
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player") && isGimmickEnable)
        {
            StopCoroutine(_tornado);
            _tornado = null;
            GameManager.instance.KillBalloon();
        }
    }
    
    private IEnumerator PullObject(Component x)
    {
        while (true)
        {
            var foreDir = transform.position - x.transform.position;
            foreDir.y = 0;
            var dist = foreDir.magnitude;
            foreDir.Normalize();

            var finalRotate = Mathf.Clamp(rotationDegree - dist, 0, rotationDegree);
            var rotatedForeDir = new Vector3(
                foreDir.x * Mathf.Cos(finalRotate) - foreDir.z * Mathf.Sin(finalRotate),
                0,
                foreDir.x * Mathf.Sin(finalRotate) + foreDir.z * Mathf.Cos(finalRotate)
            ) * pullPower;
            rotatedForeDir.y = upPower;
            x.GetComponent<Rigidbody>().AddForce(rotatedForeDir * Time.deltaTime);
            yield return refreshRate;
        }
    }
}
