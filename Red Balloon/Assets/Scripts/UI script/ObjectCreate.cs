using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreate : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private float term;
    private float _t;

    private void Awake()
    {
        _t = 0;
    }
    // Update is called once per frame
    void Update()
    {
        _t += Time.deltaTime;
        if(_t > term)
        {
            Instantiate(obj);
            _t = 0;
        }
    }
}
