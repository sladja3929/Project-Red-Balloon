using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Stage3CutScene : CutScene
{
    [SerializeField] private GameObject balloonDummy;
    [SerializeField] private GameObject balloonMan;
    [SerializeField] private float moveSpeed = 5f;

    public float t1;
    public float t2;
    public float t3;
    
    private bool isMove = false;
    private bool isPlayed;
    
    protected override void Awake()
    {
        base.Awake();
        isPlayed = false;
    }
    
    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        
        if (hasExecuted && !isPlayed)
        {
            SoundManager.instance.FadeOutBackgroundVolume();
            myCoroutine = StartCoroutine("PlayCutScene");
            isPlayed = true;
        }
    }
    
    private IEnumerator PlayCutScene()
    {
        //init
        GameManager.instance.CinematicMode();
        FadingInfo fadingInfo = new FadingInfo(2f, 0, 1, 0);
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;
        
        //disable player, activate dummy
        SceneChangeManager.instance.FadeIn(fadingInfo);
        balloonDummy.SetActive(true);
        GameObject.FindWithTag("Player").SetActive(false);
        
        //first camera, first rotation
        cameraMovements[0].cutSceneCamera.Priority = 14;
        cameraMovements[0].dollyCart.m_Speed = 0.075f;
        cameraMovements[0].dollyCart.enabled = true;
        yield return new WaitForSeconds(t1); //5
        
        //first camera slow
        cameraMovements[0].dollyCart.m_Speed = t2;//0.005f;
        yield return new WaitForSeconds(t3); //1.5
        
        //balloonman birth
        balloonMan.SetActive(true);
        balloonDummy.SetActive(false);
        balloonMan.GetComponent<Animator>().SetTrigger("morph");
        yield return new WaitForSeconds(3.5f);
        
        //first camera, second rotation
        cameraMovements[0].dollyCart.m_Speed = 0.25f;
        cameraMovements[0].dollyCart.enabled = true;
        yield return new WaitForSeconds(2.5f);
        
        //balloonman walk
        yield return new WaitForSeconds(2.5f);
        balloonMan.GetComponent<Animator>().SetTrigger("walk");
        isMove = true;
        yield return new WaitForSeconds(6f);
        
        
        fadingInfo.playTime = 4f;
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;
        
        //second camera
        fadingInfo.playTime = 2f;
        SceneChangeManager.instance.FadeIn(fadingInfo);
        cameraMovements[1].cutSceneCamera.Priority = 16;
        
        yield return waitingFadeFinish;
        
        
        
        
        /*        
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;
        
        //change scene
        fadingInfo.playTime = 5;
        void FadeIn() => SceneChangeManager.instance.FadeIn(fadingInfo);
        SceneChangeManager.instance.LoadSceneAsync("stage3", onFinish: FadeIn);*/
    }

    private void FixedUpdate()
    {
        if (isMove)
        {
            balloonMan.transform.Translate(-balloonMan.transform.right * moveSpeed * Time.fixedDeltaTime); 
        }
    }
}