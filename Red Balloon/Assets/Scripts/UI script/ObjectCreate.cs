using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreate : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private float term;
    private float t;

    private void Awake()
    {
        t = 0;
    }
    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if(t > term)
        {
            Instantiate(obj);
            t = 0;
        }
    }
}
