using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaySetter : MonoBehaviour
{
    public Vector3 pivot;
    // Update is called once per frame
    void Update()
    {
        var tsf = transform;
        
        tsf.localPosition = Vector3.zero;
        Vector3 originalPosition = tsf.position;
        
        tsf.localPosition = pivot;
        Vector3 targetPosition = tsf.position;

        Vector3 direction = targetPosition - originalPosition;
        direction.Normalize();

        RaycastHit hit;
        if(Physics.Raycast(originalPosition, direction, out hit, pivot.magnitude, LayerMask.GetMask("Platform")))
        {
            tsf.position = originalPosition + direction * hit.distance;
        }
        else
        {
            tsf.position = targetPosition;
        }
    }
}
