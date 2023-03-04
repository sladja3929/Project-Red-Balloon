using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    public Transform tornadoCenter;
    public float pullPower;
    public float refreshRate;
    
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

    private IEnumerator PullObject(Collider x, bool shouldPull)
    {
        if (shouldPull)
        {
            var foreDir = tornadoCenter.position - x.transform.position;
            x.GetComponent<Rigidbody>().AddForce(foreDir.normalized * (pullPower * Time.deltaTime));
            yield return refreshRate;
            StartCoroutine(PullObject(x, shouldPull));
        }
    }
}
