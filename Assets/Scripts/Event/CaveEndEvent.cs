using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveEndEvent : MonoBehaviour
{
    [SerializeField] private GameObject ambientLight;
    [SerializeField] private float intensity;
    [SerializeField] private float fadeTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !ambientLight.activeSelf)
        {
            ambientLight.SetActive(true);
            StartCoroutine(SetAmbientLight());
        }
    }
    
    private IEnumerator SetAmbientLight()
    {
        float time = 0;
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            ambientLight.GetComponent<Light>().intensity = Mathf.Lerp(0, intensity, time / fadeTime);
            yield return null;
        }
    }
}
