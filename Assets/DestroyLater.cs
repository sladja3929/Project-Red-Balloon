using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLater : MonoBehaviour
{
    [SerializeField] private float waitTime;
    private void Start()
    {
        StartCoroutine(WaitCoroutine());
    }

    private IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
