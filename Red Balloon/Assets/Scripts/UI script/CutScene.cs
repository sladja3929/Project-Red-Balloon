using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
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

    [SerializeField] private FadingInfo tutorialFadingInfo;

    private float _curTime;
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
            StartCoroutine(TutorialCutScene());
        }
    }

    private IEnumerator TutorialCutScene()
    {
        FadingInfo cutSceneFadeInfo = new FadingInfo(1, 0, 1, 0);
        WaitUntil waitingFadeFinish = new WaitUntil(SceneChangeManager.instance.FinishFade); 
        void FadeIn() => SceneChangeManager.instance.FadeIn(cutSceneFadeInfo);
        void FadeOut() => SceneChangeManager.instance.FadeOut(cutSceneFadeInfo);

        
        //float time = 0;
        // SceneChangeManager.Instance.SetTime(1f, 0f);
        // SceneChangeManager.Instance.SetAlpha(0f, 1f);
        // yield return SceneChangeManager.Instance.StartCoroutine(SceneChangeManager.Instance.FadeInCoroutine());

        FadeOut();
        yield return waitingFadeFinish;


        // SceneChangeManager.Instance.SetTime(1f, 1f);
        // yield return SceneChangeManager.Instance.StartCoroutine(SceneChangeManager.Instance.FadeOutCoroutine());
        
        FadeIn();
        StartCoroutine(CameraMoving());

        yield return new WaitUntil(() => isCameraMoving is not true);

        FadeOut();
        yield return waitingFadeFinish;
        
        //while(time < timeToMove - 1f)
        //{
        //    time += Time.deltaTime;
        //    yield return null;
        //}

        //SceneChangeManager.Instance.SetTime(1f, 5f);
        //yield return SceneChangeManager.instance.StartCoroutine(SceneChangeManager.instance.FadeInCoroutine());
        
        //SceneChangeManager.instance.StartCoroutine(nameof(SceneChangeManager.LoadSceneAsyncCoroutine), "Stage1");
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

    [SerializeField] private bool isCameraMoving;
    private IEnumerator CameraMoving()
    {
        isCameraMoving = true;
        
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
            
            _curTime += Time.deltaTime;
            yield return null;
        }


        isCameraMoving = false;
    }
}
