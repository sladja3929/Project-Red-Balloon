using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : Gimmick
{
    public float pullPower;
    public float upPower;
    public float refreshRate;
    [Range(0, 90)] public float rotationDegree;

    private Coroutine _tornado;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && isGimmickEnable)
        {
            if (_tornado is not null) return;
            _tornado = StartCoroutine(PullObject(col));
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player") && isGimmickEnable)
        {
            StopCoroutine(_tornado);
            _tornado = null;
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
