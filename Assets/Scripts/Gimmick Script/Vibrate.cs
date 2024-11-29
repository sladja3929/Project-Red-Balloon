using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Vibrate : Gimmick
{
    [SerializeField] private float vibrateDuration = 2.0f;
    [SerializeField] private float vibrateMagnitude = 0.05f;
    [SerializeField] private float delay = 5f;
    
    // Original position and rotation
    private Vector3 originalPos;
    
    void Awake()
    {
        originalPos = transform.localPosition;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Execute();
        }
    }
    
    public override void Execute()
    {
        if (!isGimmickEnable) return;
        
        StartCoroutine("VibrateCoroutine");
    }
    
    private IEnumerator VibrateCoroutine()
    {
        yield return new WaitForSeconds(delay);
        float elapsed = 0.0f;

        while (elapsed < vibrateDuration)
        {
            Vector3 newPos = originalPos;

            // Generate a random offset for position
            newPos += Random.insideUnitSphere * vibrateMagnitude;

            // Apply the new position and rotation
            transform.localPosition = newPos;

            // Increment elapsed time
            elapsed += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
