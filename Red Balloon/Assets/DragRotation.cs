using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DragRotation : MonoBehaviour
{
    public bool onControll = false;
    
    public float rotationSpeed = 10f;

    public Camera cam;
    private void DragRotate()
    {

        if (Input.GetMouseButton(0))
        {
            float rotx = Input.GetAxis("Mouse X") * rotationSpeed;
            float roty = Input.GetAxis("Mouse Y") * rotationSpeed;

            Vector3 right = Vector3.Cross(cam.transform.up, transform.position - cam.transform.position);
            Vector3 up = Vector3.Cross(transform.position - cam.transform.position, right);
            transform.rotation = Quaternion.AngleAxis(-rotx, up) * transform.rotation;
            transform.rotation = Quaternion.AngleAxis(roty, right) * transform.rotation;
        }
        
        
        //테스트용 전방 45도 바라보는 방향지정
        if (Input.GetKey(KeyCode.Y))
        {
            transform.rotation = Quaternion.Euler(-45, 0, 0);
        }
    }

    void Update()
    {
        if (onControll) DragRotate();
    }

}
