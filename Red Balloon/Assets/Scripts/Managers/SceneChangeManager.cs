using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
/// 씬 관리 클래스
/// SceneManager는 유니티 SceneManagement에 있어 SceneChangeManager라고 명명
/// 씬 전환, 페이드 인, 페이드 아웃 담당
/// 싱글톤 상속
*/

public class SceneChangeManager : Singleton<SceneChangeManager>
{
    //씬 로드

    public IEnumerator LoadSceneAsync(string nextSceneName)
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
                yield return StartCoroutine(nameof(FadeOut));
            }

            yield return null;
        }
        
        GameManager.Instance.StartGame();
    }
    //페이드 인 아웃
    private Image _fadeImage;

    private float _playTime;
    private float _delayTime;
    private float _lightAlpha;     //알파 비율 0 ~ 1
    private float _thickAlpha;
    //튜토리얼 씬 로드시 실행
    private void Awake()
    {
        base.Awake();
        _fadeImage = gameObject.GetComponentsInChildren<Image>()[0];
    }

    public void SetTime(float playTime, float delayTime)
    {
        _playTime = playTime;
        _delayTime = delayTime;
    }

    public void SetAlpha(float lightAlpha, float thickAlpha)
    {
        _lightAlpha = lightAlpha;
        _thickAlpha = thickAlpha;
    }

    private void ChangeAlpha(float alpha)
    {
        Color color = _fadeImage.color;
        color.a = alpha;
        _fadeImage.color = color;
    }

    public IEnumerator FadeIn()
    {
        float t = 0f;
        float start, end;

        start = _lightAlpha;
        end = _thickAlpha;
        ChangeAlpha(_lightAlpha);

        yield return new WaitForSeconds(_delayTime);

        while (t < 1)
        {
            t += Time.deltaTime / _playTime;
            ChangeAlpha(Mathf.Lerp(start, end, t));

            yield return null;
        }
    }
    
    public IEnumerator FadeOut()
    {
        float t = 0f;
        float start, end;
            
        start = _thickAlpha;
        end = _lightAlpha;
        ChangeAlpha(_thickAlpha);

        yield return new WaitForSeconds(_delayTime);

        while (t < 1)
        {
            t += Time.deltaTime / _playTime;
            ChangeAlpha(Mathf.Lerp(start, end, t));

            yield return null;
        }
    }
}