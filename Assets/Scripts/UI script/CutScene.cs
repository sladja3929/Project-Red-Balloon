using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CutScene : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cutSceneCamera;
    [SerializeField] private CinemachineVirtualCamera cutSceneCamera2;
    [SerializeField] private CinemachineDollyCart dollyCart;
    [SerializeField] private ParticleSystem particleObject;
    [SerializeField] private float timeToMove;
    [SerializeField] private bool hasToStay;

    WaitUntil waitingFadeFinish;
    private float _curTime;
    private Vector3[] _curvePoints;
    private bool _isRotation;
    
    private Coroutine myCoroutine;
    private bool hasExecuted;
    private void Awake()
    {
        waitingFadeFinish = new WaitUntil(SceneChangeManager.instance.FinishFade);
        dollyCart.m_Speed = 1f / timeToMove;
        dollyCart.enabled = false;
        hasExecuted = false;
    }

    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && hasToStay && !hasExecuted)
        {
            if (GameManager.instance.CanBalloonMove())
            {
                myCoroutine = StartCoroutine(SceneManager.GetActiveScene().name + "CutScene");
                hasExecuted = true;
                GameManager.instance.FreezeBalloon();
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !hasToStay)
        {
            myCoroutine = StartCoroutine(SceneManager.GetActiveScene().name + "CutScene");  
        }
    }
    
    private IEnumerator Stage1CutScene()
    {
        FadingInfo fadingInfo = new FadingInfo(1f, 0, 1, 0);
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;
        
        SceneChangeManager.instance.FadeIn(fadingInfo);
        cutSceneCamera.Priority = 14;
        _isRotation = true;
        dollyCart.enabled = true;
        //particleObject.Play();
        yield return new WaitForSeconds(timeToMove - 1f);

        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;
        
        SceneChangeManager.instance.FadeIn(fadingInfo);
        cutSceneCamera2.Priority = 16;
        yield return new WaitForSeconds(1f);
        
        yield return new WaitForSeconds(1f);
        
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;
        
        fadingInfo.playTime = 5;
        void FadeIn() => SceneChangeManager.instance.FadeIn(fadingInfo);
        SceneChangeManager.instance.LoadSceneAsync("stage2", onFinish: FadeIn);
    }

    private IEnumerator Stage0CutScene()
    {
        Debug.Log("cut0");
        FadingInfo fadingInfo = new FadingInfo(1, 0, 1, 0);
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;
        
        fadingInfo.delayTime = 0.5f;
        SceneChangeManager.instance.FadeIn(fadingInfo);
        cutSceneCamera.Priority = 14;
        _isRotation = true;
        dollyCart.enabled = true;
        yield return new WaitForSeconds(timeToMove - 0.5f);

        fadingInfo.delayTime = 0;
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;

        fadingInfo.playTime = 5;
        void FadeIn() => SceneChangeManager.instance.FadeIn(fadingInfo);
        SceneChangeManager.instance.LoadSceneAsync("stage1", onFinish : FadeIn);
    }

    enum CameraState
    {
        Stop,
        Moving,
        AlmostFinish,
    }
    
    [SerializeField] private CameraState _cutSceneCameraState;
}
