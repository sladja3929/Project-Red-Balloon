using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIsland : MonoBehaviour
{
    [SerializeField] private bool isLeft = false;
    [SerializeField] private float rotateSpeed = 1;
    [SerializeField] private float acceleration = 0.1f;

    private short dir = -1;
    private Vector3 angle;
    
    private GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        if (isLeft) dir = 1;
        angle = parent.transform.eulerAngles;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //acceleration += acceleration;
            angle.z = angle.z + dir * (rotateSpeed + acceleration);
            parent.transform.eulerAngles = angle;
        }
    }
}
