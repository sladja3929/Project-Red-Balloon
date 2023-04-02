//토네이도 수정본
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    public Transform tornadoCenter;
    public float pullPower;
    public float upPower;
    public float refreshRate;

    public float rotationDegree;
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(PullObject(col, true));
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(PullObject(col, false));
        }
    }

    private IEnumerator PullObject(Component x, bool shouldPull)
    {
        if (!shouldPull) yield break;
        
        var foreDir = tornadoCenter.position - x.transform.position;
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
        StartCoroutine(PullObject(x, true));
    }
}
