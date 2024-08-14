using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIsland : MonoBehaviour
{
    [SerializeField] private bool isLeft = false;

    private GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isLeft) parent.transform.eulerAngles = new Vector3(90, 0, 0);
        }
    }
}
