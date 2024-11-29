using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using Unity.VisualScripting;

public class Stage0CutScene : CutScene
{
    protected override void Awake()
    {
        base.Awake();
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (hasExecuted)
        {
            myCoroutine = StartCoroutine("PlayCutScene");
        }
    }
    
    private IEnumerator PlayCutScene()
    {
        GameManager.instance.CinematicMode();
        FadingInfo fadingInfo = new FadingInfo(1, 0, 1, 0);
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;
        
        fadingInfo.delayTime = 0.5f;
        SceneChangeManager.instance.FadeIn(fadingInfo);
        cameraMovements[0].cutSceneCamera.Priority = 14;
        _isRotation = true;
        cameraMovements[0].dollyCart.enabled = true;
        yield return new WaitForSeconds(cameraMovements[0].timeToMove - 0.5f);

        fadingInfo.delayTime = 0;
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;

        fadingInfo.playTime = 5;
        void FadeIn() => SceneChangeManager.instance.FadeIn(fadingInfo);
        SceneChangeManager.instance.LoadSceneAsync("stage1", onFinish : FadeIn);
    }
}
