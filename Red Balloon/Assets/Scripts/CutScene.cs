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
    private SceneStreamingTrigger sceneStreamingTrigger;
    private FadeInOut fadeInOut;

    private void Start()
    {
        cutSceneCamera.enabled = false;
        sceneStreamingTrigger = GetComponent<SceneStreamingTrigger>();
        fadeInOut = GetComponent<FadeInOut>();
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
        fadeInOut.SetTime(0.5f, 0f);
        yield return fadeInOut.StartCoroutine("Fade", "In");
        fadeInOut.SetTime(1f, 0.5f);
        fadeInOut.StartCoroutine("Fade", "Out");
        yield return StartCoroutine("CameraMoving");

        //sceneStreamingTrigger.StartCoroutine("UnloadStreamingScene");
        fadeInOut.SetTime(1f, 0f);
        fadeInOut.StartCoroutine("Fade", "Out");
        //yield return sceneStreamingTrigger.StartCoroutine("StreamingTargetScene");
        fadeInOut.StartCoroutine("Fade", "In");
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
