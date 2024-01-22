using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLooking : MonoBehaviour
{
    public GameObject target;
    
    void Update()
    {
        if(target != null)
            transform.LookAt(target.transform);
    }
}
