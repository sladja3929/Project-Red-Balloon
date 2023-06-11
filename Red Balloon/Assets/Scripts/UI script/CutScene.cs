using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    [SerializeField] private Camera cutSceneCamera;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform pointC;
    [SerializeField] private Transform pointD;

    [SerializeField] private float timeToMove;
    [SerializeField] private float rotationSpeed;

    WaitUntil waitingFadeFinish;
    private float _curTime;
    private Vector3[] _curvePoints;

    private void Start()
    {
        waitingFadeFinish = new WaitUntil(SceneChangeManager.instance.FinishFade);
        cutSceneCamera.enabled = false;
    }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(SceneManager.GetActiveScene().name + "CutScene");
        }
    }

    private IEnumerator Stage1CutScene()
    {
        FadingInfo cutSceneFadeInfo = new FadingInfo(1, 0, 1, 0);
        WaitUntil waitingFadeFinish = new WaitUntil(SceneChangeManager.instance.FinishFade);
        void FadeIn() => SceneChangeManager.instance.FadeIn(cutSceneFadeInfo);
        void FadeOut() => SceneChangeManager.instance.FadeOut(cutSceneFadeInfo);

        yield return null;
    }

    private IEnumerator Stage0CutScene()
    {
        SceneChangeManager.instance.FadeOut(new FadingInfo(1, 0, 1, 0));
        yield return waitingFadeFinish;

        SceneChangeManager.instance.FadeIn(new FadingInfo(1, 1, 1, 0));
        StartCoroutine(CameraMoving());

        yield return new WaitUntil(() => _cutSceneCameraState is CameraState.Stop or CameraState.AlmostFinish);

        SceneChangeManager.instance.FadeOut(new FadingInfo(1, 0, 1, 0));
        yield return waitingFadeFinish;

        void FadeIn() => SceneChangeManager.instance.FadeIn(new FadingInfo(5, 0, 1, 0));
        SceneChangeManager.instance.LoadSceneAsync("stage1", onFinish : FadeIn);
    }

    private Vector3 CalculateBezierPoint()
    {
        float percentage = _curTime / timeToMove;
        
        Vector3 pA = pointA.position;
        Vector3 pB = pointB.position;
        Vector3 pC = pointC.position;

        return Vector3.Lerp(Vector3.Lerp(pA, pB, percentage), Vector3.Lerp(pB, pC, percentage), percentage);
    }

    enum CameraState
    {
        Stop,
        Moving,
        AlmostFinish,
    }
    
    [SerializeField] private CameraState _cutSceneCameraState;
    private IEnumerator CameraMoving()
    {
        _cutSceneCameraState = CameraState.Moving;
        
        _curTime = 0f;
        Camera.main.enabled = false;
        cutSceneCamera.enabled = true;

        while(_curTime < timeToMove)
        {
            cutSceneCamera.transform.position = CalculateBezierPoint();
            
            cutSceneCamera.transform.rotation =
                Quaternion.Lerp(cutSceneCamera.transform.rotation, 
                    Quaternion.LookRotation(pointD.position - cutSceneCamera.transform.position), 
                    Time.deltaTime * rotationSpeed);

            if (_curTime > timeToMove * 0.9f) _cutSceneCameraState = CameraState.AlmostFinish;
            _curTime += Time.deltaTime;
            yield return null;
        }


        _cutSceneCameraState = CameraState.Stop;
    }
}
