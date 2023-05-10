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

    private void Start()
    {
        cutSceneCamera.enabled = false;
    }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine("TutorialCutScene");
        }
    }

    private IEnumerator TutorialCutScene()
    {
        float time = 0;
        SceneChangeManager.Instance.SetTime(1f, 0f);
        SceneChangeManager.Instance.SetAlpha(0f, 1f);
        yield return SceneChangeManager.Instance.StartCoroutine("Fade", "In");

        StartCoroutine("CameraMoving");

        SceneChangeManager.Instance.SetTime(1f, 1f);
        yield return SceneChangeManager.Instance.StartCoroutine("Fade", "Out");
        
        //while(time < timeToMove - 1f)
        //{
        //    time += Time.deltaTime;
        //    yield return null;
        //}

        SceneChangeManager.Instance.SetTime(1f, 5f);
        yield return SceneChangeManager.Instance.StartCoroutine("Fade", "In");
        SceneChangeManager.Instance.SetTime(6f, 0f);
        SceneChangeManager.Instance.StartCoroutine("LoadSceneAsync", "Stage1");

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
        Camera.main.enabled = false;
        cutSceneCamera.enabled = true;
        while(t < 1)
        {            
            cutSceneCamera.transform.position = CalculateBezierPoint();
            cutSceneCamera.transform.rotation = Quaternion.Lerp(cutSceneCamera.transform.rotation, Quaternion.LookRotation(pointD.position - cutSceneCamera.transform.position), Time.deltaTime * rotationSpeed);
            t += Time.deltaTime / timeToMove;
            yield return null;
        }
        
    }
}
