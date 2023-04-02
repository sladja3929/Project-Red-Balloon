using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CalculateVector : MonoBehaviour
{
    public GameObject baseObject;
    public GameObject targetObject;

    private void OnDrawGizmos()
    {
        Vector3 V = (targetObject.transform.position - baseObject.transform.position).normalized;
        Debug.Log(V);
    }
}
