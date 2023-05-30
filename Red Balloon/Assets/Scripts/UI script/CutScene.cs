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

    private float _t;
    private Vector3[] _curvePoints;

    private void Start()
    {
        cutSceneCamera.enabled = false;
    }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(nameof(TutorialCutScene));
        }
    }

    private IEnumerator TutorialCutScene()
    {
        float time = 0;
        SceneChangeManager.Instance.SetTime(1f, 0f);
        SceneChangeManager.Instance.SetAlpha(0f, 1f);
        yield return SceneChangeManager.Instance.StartCoroutine(nameof(FadeInOut));

        StartCoroutine("CameraMoving");

        SceneChangeManager.Instance.SetTime(1f, 1f);
        yield return SceneChangeManager.Instance.StartCoroutine(nameof(SceneChangeManager.FadeOut));
        
        //while(time < timeToMove - 1f)
        //{
        //    time += Time.deltaTime;
        //    yield return null;
        //}

        SceneChangeManager.Instance.SetTime(1f, 5f);
        yield return SceneChangeManager.Instance.StartCoroutine(nameof(SceneChangeManager.FadeIn));
        SceneChangeManager.Instance.SetTime(6f, 0f);
        SceneChangeManager.Instance.StartCoroutine(nameof(SceneChangeManager.LoadSceneAsync), "Stage1");

    }

    private Vector3 CalculateBezierPoint()
    {
        Vector3 pA = pointA.position;
        Vector3 pB = pointB.position;
        Vector3 pC = pointC.position;

        return Vector3.Lerp(Vector3.Lerp(pA, pB, _t), Vector3.Lerp(pB, pC, _t), _t);
    }

    private IEnumerator CameraMoving()
    {
        _t = 0f;
        Camera.main.enabled = false;
        cutSceneCamera.enabled = true;
        while(_t < timeToMove)
        {            
            cutSceneCamera.transform.position =
                CalculateBezierPoint();
            
            cutSceneCamera.transform.rotation =
                Quaternion.Lerp(cutSceneCamera.transform.rotation, 
                    Quaternion.LookRotation(pointD.position - cutSceneCamera.transform.position), 
                    Time.deltaTime * rotationSpeed);
            
            _t += Time.deltaTime;
            yield return null;
        }
        
    }
}
