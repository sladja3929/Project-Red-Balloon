using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 페이드 인 아웃관련 정보를 담고있는 구조체입니다.
/// </summary>
[Serializable]
public struct FadingInfo
{
    /// <summary>
    /// 페이드가 진행되는 총 시간입니다.
    /// </summary>
    public float playTime;
    
    /// <summary>
    /// 해당 초 이후에 페이드 인/아웃이 진행됩니다.
    /// </summary>
    public float delayTime;
    
    /// <summary>
    /// 가장 밝은 순간의 alpha 값입니다.
    /// </summary>
    public float lightAlpha;
    
    /// <summary>
    /// 가장 어두운 순간의 alpha 값입니다.
    /// </summary>
    public float thickAlpha;

    public FadingInfo(float playTime, float delayTime, float lightAlpha, float thickAlpha)
    {
        this.thickAlpha = thickAlpha;
        this.delayTime = delayTime;
        this.playTime = playTime;
        this.lightAlpha = lightAlpha;
    }
}

/// <summary>씬 관리 클래스
/// SceneManager는 유니티 SceneManagement에 있어 SceneChangeManager라고 명명
/// 씬 전환, 페이드 인, 페이드 아웃 담당
/// 싱글톤 상속
/// </summary>
public class SceneChangeManager : Singleton<SceneChangeManager>
{
    // private float _playTime;
    // private float _delayTime;
    // private float _lightAlpha;     //알파 비율 0 ~ 1
    // private float _thickAlpha;
    [SerializeField] private FadingInfo basicInfo;
    
    //씬 로드
    public void LoadSceneAsync(string nextSceneName, Action onFinish = null)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(nextSceneName, onFinish));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string nextSceneName, Action onFinish = null)
    {
        // Load scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false;

        // Update loading text until scene is fully loaded
        while (!asyncLoad.isDone)
        {

            if (asyncLoad.progress >= 0.9f)
            {
                // Activate the new scene
                asyncLoad.allowSceneActivation = true;

                // Start fade out
                // yield return StartCoroutine(FadeOutCoroutine(basicInfo));
            }

            yield return null;
        }

        onFinish?.Invoke();
    }
    //페이드 인 아웃
    private Image _fadeImage;

    

    public bool isFading;
    //튜토리얼 씬 로드시 실행
    private new void Awake()
    {
        base.Awake();
        _fadeImage = gameObject.GetComponentsInChildren<Image>()[0];
        isFading = false;
    }

    // public void SetTime(float playTime, float delayTime)
    // {
    //     // _playTime = playTime;
    //     // _delayTime = delayTime;
    // }
    //
    // public void SetAlpha(float lightAlpha, float thickAlpha)
    // {
    //     // _lightAlpha = lightAlpha;
    //     // _thickAlpha = thickAlpha;
    // }

    private void ChangeAlpha(float alpha)
    {
        Color color = _fadeImage.color;
        color.a = alpha;
        _fadeImage.color = color;
    }
    
    public void FadeIn(FadingInfo info)
    {
        if (isFading)
        {
            Debug.LogError("is fading now, cant call");
        }
        
        StartCoroutine(FadeInCoroutine(info));
    }

    public void FadeOut(FadingInfo info)
    {
        if (isFading)
        {
            Debug.LogError("is fading now, cant call");
        }
        
        StartCoroutine(FadeOutCoroutine(info));
    }

    private IEnumerator FadeInCoroutine(FadingInfo info)
    {
        var yieldWaitForSeconds = new WaitForSeconds(info.delayTime);
        float startAlpha = info.lightAlpha;
        float endAlpha = info.thickAlpha;
        float oneReversePlayTime = 1 / info.playTime; //나누기 반복하는건 계산상 비효율적
        ChangeAlpha(startAlpha);
        
        float curTime = 0f;
        
        isFading = true;
        
        yield return yieldWaitForSeconds;

        while (curTime < 1)
        {
            curTime += Time.deltaTime * oneReversePlayTime;
            ChangeAlpha(Mathf.Lerp(startAlpha, endAlpha, curTime));

            yield return null;
        }

        isFading = false;
    }

    private IEnumerator FadeOutCoroutine(FadingInfo info)
    {
        var yieldWaitForSeconds = new WaitForSeconds(info.delayTime);
        float startAlpha = info.thickAlpha;
        float endAlpha = info.lightAlpha;
        float oneReversePlayTime = 1 / info.playTime; //나누기 반복하는건 계산상 비효율적
        ChangeAlpha(startAlpha);
        
        float curTime = 0f;
        
        isFading = true;
        
        yield return yieldWaitForSeconds;

        while (curTime < 1)
        {
            curTime += Time.deltaTime * oneReversePlayTime;
            ChangeAlpha(Mathf.Lerp(startAlpha, endAlpha, curTime));

            yield return null;
        }
        
        isFading = false;
    }

    public bool FinishFade() => isFading is not true;
}