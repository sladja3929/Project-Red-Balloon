using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class DragRotation : MonoBehaviour
{
    public bool onControll = false;
    
    public float rotationSpeed = StaticSensitivity.mouseSensitivity;
    
    public Camera cam;

    public GameObject direction;

    public bool isOnFlyMode;
    
    private void DragRotate()
    {

        if (Input.GetMouseButton(0))
        {
            //cam = Camera.main;
            //direction = transform.parent.GetChild(2).gameObject;

            float rotx = Input.GetAxis("Mouse X") * rotationSpeed;
            float roty = Input.GetAxis("Mouse Y") * rotationSpeed;

            // Calculate rotation axes using a more stable approach
            Vector3 right = Vector3.Cross(cam.transform.up, direction.transform.forward);
            Vector3 up = cam.transform.up;

            // Apply rotations using Quaternions to avoid gimbal lock
            Quaternion xRotation = Quaternion.AngleAxis(-rotx, up);
            Quaternion yRotation = Quaternion.AngleAxis(roty, right);
            direction.transform.rotation = yRotation * xRotation * direction.transform.rotation;
        }
        
        
        //테스트용 전방 45도 바라보는 방향지정
        if (Input.GetKey(KeyCode.Y))
        {
            direction.transform.rotation = Quaternion.Euler(-45, 0, 0);
        }
    }

    void Update()
    {
        if (GameManager.IsPause) return;
        
        if (onControll) DragRotate();
        if (isOnFlyMode) transform.rotation = direction.transform.rotation;
    }

    public Quaternion GetDirection()
    {
        return direction.transform.rotation;
    }

    public void ResetDirection()
    {
        Vector3 dir = cam.transform.localRotation.eulerAngles;
        dir.x -= 30f;
        direction.transform.localRotation = Quaternion.Euler(dir);
    }
    
    public void SetRotationSpeedRate(float rate)
    {
        rotationSpeed = Mathf.Lerp(minSpeed, maxSpeed, rate);
    }
    
    public float GetRotationSpeedRate()
    {
        return (rotationSpeed - minSpeed) / (maxSpeed - minSpeed);
    }
}
