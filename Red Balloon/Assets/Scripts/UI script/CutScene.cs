using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    [SerializeField] private Camera cutSceneCamera;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform middlePoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform lookPoint;
    [SerializeField] private Transform nextPoint;

    [SerializeField] private float timeToMove;
    [SerializeField] private float rotationSpeed;

    WaitUntil waitingFadeFinish;
    private float _curTime;
    private Vector3[] _curvePoints;
    private bool _isRotation;

    private void Start()
    {
        waitingFadeFinish = new WaitUntil(SceneChangeManager.instance.FinishFade);
        cutSceneCamera.transform.position = startPoint.transform.position;
        cutSceneCamera.transform.rotation = Quaternion.LookRotation(lookPoint.position - cutSceneCamera.transform.position).normalized;        
        cutSceneCamera.enabled = false;
        lookPoint.gameObject.SetActive(false);
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
        FadingInfo fadingInfo = new FadingInfo(1, 0, 1, 0);
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;

        SceneChangeManager.instance.FadeIn(fadingInfo);
        _isRotation = true;
        StartCoroutine(CameraMovingCoroutine());
        yield return new WaitUntil(() => _cutSceneCameraState is CameraState.Stop or CameraState.AlmostFinish);

        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;

        
        fadingInfo.delayTime = 1;
        SceneChangeManager.instance.FadeIn(fadingInfo);
        yield return waitingFadeFinish;

        fadingInfo.delayTime = 5;
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;

        fadingInfo.delayTime = 0;
        fadingInfo.playTime = 4;
        void FadeIn() => SceneChangeManager.instance.FadeIn(fadingInfo);
        //SceneChangeManager.instance.LoadSceneAsync("stage2", onFinish: FadeIn);
    }

    private IEnumerator Stage0CutScene()
    {
        FadingInfo fadingInfo = new FadingInfo(1, 0, 1, 0);
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;

        fadingInfo.delayTime = 0.5f;
        SceneChangeManager.instance.FadeIn(fadingInfo);
        _isRotation = true;
        StartCoroutine(CameraMovingCoroutine());
        yield return new WaitUntil(() => _cutSceneCameraState is CameraState.Stop or CameraState.AlmostFinish);

        fadingInfo.delayTime = 0;
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;

        fadingInfo.playTime = 5;
        void FadeIn() => SceneChangeManager.instance.FadeIn(fadingInfo);
        SceneChangeManager.instance.LoadSceneAsync("stage1", onFinish : FadeIn);
    }

    private Vector3 CalculateBezierPoint()
    {
        float percentage = _curTime / timeToMove;
        
        Vector3 pA = startPoint.position;
        Vector3 pB = middlePoint.position;
        Vector3 pC = endPoint.position;

        return Vector3.Lerp(Vector3.Lerp(pA, pB, percentage), Vector3.Lerp(pB, pC, percentage), percentage);
    }

    enum CameraState
    {
        Stop,
        Moving,
        AlmostFinish,
    }
    
    [SerializeField] private CameraState _cutSceneCameraState;
    private IEnumerator CameraMovingCoroutine()
    {
        lookPoint.gameObject.SetActive(true);
        _cutSceneCameraState = CameraState.Moving;
        
        _curTime = 0f;
        Camera.main.enabled = false;
        cutSceneCamera.enabled = true;

        while(_curTime < timeToMove)
        {
            cutSceneCamera.transform.position = CalculateBezierPoint();
            
            if(_isRotation)
            {
                cutSceneCamera.transform.rotation = Quaternion.Lerp(cutSceneCamera.transform.rotation,
                                                                    Quaternion.LookRotation(lookPoint.position - cutSceneCamera.transform.position),
                                                                    Time.deltaTime * rotationSpeed);
            }

            if (_curTime > timeToMove - 1.3f) _cutSceneCameraState = CameraState.AlmostFinish;
            _curTime += Time.deltaTime;
            yield return null;
        }

        cutSceneCamera.transform.position = nextPoint.transform.position;
        cutSceneCamera.transform.rotation = Quaternion.LookRotation(transform.position - cutSceneCamera.transform.position).normalized;
        _cutSceneCameraState = CameraState.Stop;
    }
}
