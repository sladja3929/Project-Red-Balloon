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
                yield return StartCoroutine("Fade", "Out");
            }

            yield return null;
        }
        
        GameManager.Instance.StartGame();
    }
    //페이드 인 아웃
    private Image fadeImage;

    private float playTime;
    private float delayTime;
    private float lightAlpha;     //알파 비율 0 ~ 1
    private float thickAlpha;
    //튜토리얼 씬 로드시 실행
    private void Awake()
    {
        base.Awake();
        fadeImage = gameObject.GetComponentsInChildren<Image>()[0];
    }

    public void SetTime(float playTime, float delayTime)
    {
        this.playTime = playTime;
        this.delayTime = delayTime;
    }

    public void SetAlpha(float lightAlpha, float thickAlpha)
    {
        this.lightAlpha = lightAlpha;
        this.thickAlpha = thickAlpha;
    }

    private void ChangeAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }

    public IEnumerator Fade(string property)
    {
        float t = 0f;
        float start, end;

        if (property == "In")
        {
            start = lightAlpha;
            end = thickAlpha;
            ChangeAlpha(lightAlpha);
        }

        else
        {
            start = thickAlpha;
            end = lightAlpha;
            ChangeAlpha(thickAlpha);
        }

        yield return new WaitForSeconds(delayTime);

        while (t < 1)
        {
            t += Time.deltaTime / playTime;
            ChangeAlpha(Mathf.Lerp(start, end, t));

            yield return null;
        }
    }
}