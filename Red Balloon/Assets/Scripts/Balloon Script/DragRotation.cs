using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DragRotation : MonoBehaviour
{
    public bool onControll = false;
    
    public float rotationSpeed = 10f;

    public Camera cam;

    public GameObject direction;

    public bool isOnFlyMode;
    private void DragRotate()
    {

        if (Input.GetMouseButton(0))
        {
            //마우스 움직임 변화값을 Input으로 받아옴
            float rotx = Input.GetAxis("Mouse X") * rotationSpeed;
            float roty = Input.GetAxis("Mouse Y") * rotationSpeed;
            
            Vector3 right = Vector3.Cross(cam.transform.up, direction.transform.position - cam.transform.position);
            Vector3 up = Vector3.Cross(direction.transform.position - cam.transform.position, right);
            direction.transform.rotation = Quaternion.AngleAxis(-rotx, up) * direction.transform.rotation;
            direction.transform.rotation = Quaternion.AngleAxis(roty, right) * direction.transform.rotation;
        }
        
        
        //테스트용 전방 45도 바라보는 방향지정
        if (Input.GetKey(KeyCode.Y))
        {
            direction.transform.rotation = Quaternion.Euler(-45, 0, 0);
        }
    }

    void Update()
    {
        if (GameManager.isPause) return;
        
        if (onControll) DragRotate();
        if (isOnFlyMode) transform.rotation = direction.transform.rotation;
    }

    public Quaternion GetDirection()
    {
        return direction.transform.rotation;
    }
}
