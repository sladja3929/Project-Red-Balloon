using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class Stage3CutScene : CutScene
{
    [SerializeField] private GameObject balloonDummy;
    [SerializeField] private GameObject balloonMan;
    [SerializeField] private RectTransform endingCredit;
    [SerializeField] private GameObject final;
    
    [SerializeField] private float balloonManSpeed = 5f;
    [SerializeField] private float firstRotateSpeed = 4.952f;
    [SerializeField] private float slowSpeed = 0.002f;
    [SerializeField] private float delayTime = 1.2f;
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float endYPos = 1500f;
    
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
        
        //스팀도전과제
        SteamManager.instance.UpdateBestRecord();
        yield return waitingFadeFinish;

        //disable player, activate dummy
        SceneChangeManager.instance.FadeIn(fadingInfo);
        balloonDummy.SetActive(true);
        GameObject.FindWithTag("Player").SetActive(false);

        //first camera, first rotation
        cameraMovements[0].cutSceneCamera.Priority = 14;
        cameraMovements[0].dollyCart.m_Speed = 0.075f;
        cameraMovements[0].dollyCart.enabled = true;
        yield return new WaitForSeconds(firstRotateSpeed); //5

        //first camera slow
        cameraMovements[0].dollyCart.m_Speed = slowSpeed; //0.005f;
        yield return new WaitForSeconds(delayTime); //1.5

        //balloonman birth
        balloonMan.SetActive(true);
        balloonDummy.SetActive(false);
        balloonMan.GetComponent<Animator>().SetTrigger("morph");
        yield return new WaitForSeconds(3.5f);

        //first camera, second rotation
        cameraMovements[0].dollyCart.m_Speed = 0.390625f;
        cameraMovements[0].dollyCart.enabled = true;
        yield return new WaitForSeconds(1.6f);

        //balloonman walk
        yield return new WaitForSeconds(2.5f);
        balloonMan.GetComponent<Animator>().SetTrigger("walk");
        StartCoroutine("BalloonManMove");
        SoundManager.instance.BackgroundPlay(soundEffect[0]);
        yield return new WaitForSeconds(5f);


        fadingInfo.playTime = 2f;
        SceneChangeManager.instance.FadeOut(fadingInfo);
        yield return waitingFadeFinish;

        //second camera
        fadingInfo.playTime = 1.5f;
        SceneChangeManager.instance.FadeIn(fadingInfo);
        cameraMovements[1].cutSceneCamera.Priority = 16;
        
        //스팀도전과제
        SteamManager.instance.UpdateClearStage(3);
        SteamManager.instance.UpdateClearCount();
        SaveManager.instance.ResetSave();
        yield return waitingFadeFinish;
        
        StartCoroutine("EndingCredit");
    }

    private IEnumerator BalloonManMove()
    {
        while (endingCredit.anchoredPosition.y < endYPos + 50f)
        {
            balloonMan.transform.Translate(-balloonMan.transform.right * balloonManSpeed * Time.fixedDeltaTime);
            yield return null;
        }
    }

    private IEnumerator EndingCredit()
    {
        endingCredit.gameObject.SetActive(true);
        float originFixedT = Time.fixedDeltaTime;
        
        //크레딧 롤
        while (endingCredit.anchoredPosition.y < endYPos)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = originFixedT;
            
            if (Input.GetKey(KeyCode.Escape))
            {
                Time.timeScale = 10.0f;
                Time.fixedDeltaTime = originFixedT * Time.timeScale;
            }
            endingCredit.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
            yield return null;
        }
        
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = originFixedT;
        
        //카메라 멈춤
        cameraMovements[1].cutSceneCamera.transform.SetParent(null, true);
        
        //결과창 세팅
        Graphic[] graphics = final.GetComponentsInChildren<Graphic>();
        Button button = final.GetComponentInChildren<Button>();
        
        foreach (Graphic g in graphics)
        {
            Color color = g.color;
            color.a = 0;
            g.color = color;
        }
        final.SetActive(true);
        
        yield return new WaitForSeconds(4.5f);

        //결과창 띄움
        foreach (Graphic g in graphics)
        {
            if (g.gameObject.GetComponent<Button>() != null) yield return new WaitForSeconds(2f);
            yield return StartCoroutine(UIFadeIn(g));
        }
        
        button.interactable = true;
        button.onClick.AddListener(GoToMainMenu);
    }

    private void GoToMainMenu()
    {
        StartCoroutine(GoToMainMenuCoroutine());
    }

    private IEnumerator GoToMainMenuCoroutine()
    {
        FadingInfo fadingInfo = new FadingInfo(2f, 0, 1, 0);
        SceneChangeManager.instance.FadeOut(fadingInfo);
        SoundManager.instance.FadeOutBackgroundVolume();
        yield return waitingFadeFinish;

        fadingInfo.playTime = 0f;
        void FadeIn() => SceneChangeManager.instance.FadeIn(fadingInfo);
        SceneChangeManager.instance.LoadSceneAsync("MainMenu", onFinish: FadeIn);
    }
    
    private IEnumerator UIFadeIn(Graphic graphic)
    {
        Color color = graphic.color;
        color.a = 0;
        graphic.color = color;
        
        float elapsedTime = 0f;
        while (elapsedTime < 1.5f)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / 1.5f);
            graphic.color = color; 
            yield return null;
        }
        color.a = 1;
        graphic.color = color;
    }
}