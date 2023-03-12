using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDrawGizmos : MonoBehaviour
{
    //디버그용 스크립트
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 5f);
    }

}
