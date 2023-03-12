using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    [SerializeField] private Camera cutSceneCamera;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform pointC;
    [SerializeField] private Transform pointD;

    [SerializeField] private float timeToMove;
    [SerializeField] private float rotationSpeed;
    private float t;
    private Vector3[] curvePoints;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CameraMoving");
    }

    private Vector3 CalculateBezierPoint()
    {
        Vector3 pA = pointA.position;
        Vector3 pB = pointB.position;
        Vector3 pC = pointC.position;

        return Vector3.Lerp(Vector3.Lerp(pA, pB, t), Vector3.Lerp(pB, pC, t), t);
    }

    private IEnumerator CameraMoving()
    {
        t = 0f;
        while(t < 1)
        {            
            cutSceneCamera.transform.position = CalculateBezierPoint();
            cutSceneCamera.transform.rotation = Quaternion.Lerp(cutSceneCamera.transform.rotation, Quaternion.LookRotation(pointD.position - cutSceneCamera.transform.position), Time.deltaTime * rotationSpeed);
            t += Time.deltaTime / timeToMove;
            yield return null;
        }
        
    }
}
