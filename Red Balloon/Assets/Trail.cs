using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    private float _time = 0f;

    public GameObject trailPrefab;
    void Update()
    {
        _time += Time.deltaTime;
        if (_time > 0.1f)
        {
            _time = 0;
            Instantiate(trailPrefab, transform.position, Quaternion.identity);
        }
    }
}
