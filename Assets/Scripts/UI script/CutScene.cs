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
    [SerializeField] private CinemachineDollyCart dollyCart;
    [SerializeField] private ParticleSystem particleObject;
    [SerializeField] private float timeToMove;

    WaitUntil waitingFadeFinish;
    private float _curTime;
    private Vector3[] _curvePoints;
    private bool _isRotation;    

    private void Start()
    {
        waitingFadeFinish = new WaitUntil(SceneChangeManager.instance.FinishFade);
        //cutSceneCamera.Priority = priority;
        dollyCart.m_Speed = 1f / timeToMove;
        dollyCart.enabled = false;
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
        particleObject.Play();
        yield return waitingFadeFinish;

        SceneChangeManager.instance.FadeIn(fadingInfo);
        _isRotation = true;
        //StartCoroutine(CameraMovingCoroutine());
        
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
        SceneChangeManager.instance.LoadSceneAsync("stage2", onFinish: FadeIn);
    }

    private IEnumerator Stage0CutScene()
    {
        FadingInfo fadingInfo = new FadingInfo(1, 0, 1, 0);
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;

        fadingInfo.delayTime = 0.5f;
        SceneChangeManager.instance.FadeIn(fadingInfo);
        _isRotation = true;
        cutSceneCamera.Priority = 14;
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
