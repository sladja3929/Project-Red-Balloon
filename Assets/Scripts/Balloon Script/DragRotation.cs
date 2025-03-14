using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class DragRotation : MonoBehaviour
{
    public bool onControll = false;
    
    public float minSpeed = 1f;
    public float maxSpeed = 100f;
    public float rotationSpeed => StaticSensitivity.MouseSensitivity;
    public float rotateSmoothing = 80f;
    public bool isReverse = true;

    
    public Camera cam;

    public GameObject direction;

    public bool isOnFlyMode;
    
    private Quaternion targetRotation;
    
    private void DragRotate()
    {
        targetRotation = direction.transform.rotation;
        if (Input.GetMouseButton(0))
        {
            // 마우스 이동량에 따른 각도 계산
            float deltaX = Input.GetAxis("Mouse X") * rotationSpeed;
            float deltaY = Input.GetAxis("Mouse Y") * rotationSpeed;

            // isReverse에 따른 각도 부호 결정
            // 기존 코드: yaw에 -deltaX, pitch에 +deltaY 적용
            // isReverse가 false면 이 부호들을 반전시켜서 마우스 이동과 동일하게 회전하도록 함
            float factorX = isReverse ? -1f : 1f;
            float factorY = isReverse ? 1f  : -1f;

            // 회전 축 계산
            Vector3 upAxis = cam.transform.up;
            Vector3 rightAxis = Vector3.Cross(upAxis, direction.transform.forward).normalized;

            // 각 축의 회전 생성 (부호에 factor 적용)
            Quaternion yawRotation = Quaternion.AngleAxis(deltaX * factorX, upAxis);
            Quaternion pitchRotation = Quaternion.AngleAxis(deltaY * factorY, rightAxis);

            // 두 회전을 결합 (순서 주의: 먼저 좌우 회전, 다음 상하 회전)
            Quaternion deltaRotation = pitchRotation * yawRotation;

            // 누적 목표 회전값 업데이트
            targetRotation = deltaRotation * targetRotation;
        }

        // 현재 회전에서 목표 회전으로 Slerp 보간 적용 (부드러운 회전)
        direction.transform.rotation = Quaternion.Slerp(direction.transform.rotation, targetRotation, Time.deltaTime * rotateSmoothing);
        
        
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
    
    public float GetRotationSpeedRate()
    {
        //return (rotationSpeed - minSpeed) / (maxSpeed - minSpeed);
        return StaticSensitivity.GetMouseSensitivityRate();
    }

    public void SetRotationSpeedRate(float value)
    {
       // rotationSpeed = Mathf.Lerp(minSpeed, maxSpeed, value);
       StaticSensitivity.SetMouseSensitivity(value);
    }
}
