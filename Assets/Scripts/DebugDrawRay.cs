using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDrawRay : MonoBehaviour
{
    void OnDrawGizmos()
    {
        // 시작점과 끝점 설정
        Vector3 startPoint = transform.position;
        Vector3 endPoint = transform.position + transform.forward * 10;

        // 선 그리기
        Debug.DrawLine(startPoint, endPoint, Color.red);
    }
}