using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
    }
    
    public void ShakeCamera(float amount, float time)
    {
        StartCoroutine(ShakeCameraRoutine(amount, time));
    }

    private IEnumerator ShakeCameraRoutine(float amount, float time)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < time)
        {
            float x = Random.Range(-1f, 1f) * amount;
            float y = Random.Range(-1f, 1f) * amount;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
