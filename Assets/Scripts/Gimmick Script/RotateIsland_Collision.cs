using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIsland_Collision : MonoBehaviour
{
    [SerializeField] private bool isLeft;
    
    private RotateIsland parentComp;
    // Start is called before the first frame update
    void Start()
    {
        parentComp = transform.GetComponentInParent<RotateIsland>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if(isLeft) parentComp.EnterPlatform(-1);
            else parentComp.EnterPlatform(1);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            parentComp.ExitPlatform();
        }
    }
}
