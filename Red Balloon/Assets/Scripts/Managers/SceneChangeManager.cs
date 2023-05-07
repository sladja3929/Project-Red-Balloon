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
    private string nextSceneName = "stage1";

    public IEnumerator LoadSceneAsync()
    {
        // Load scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false;

        // Start fade in
        //SetTime(2f, 0f);
        //yield return StartCoroutine("Fade", "In");
        //yield return new WaitForSeconds(1f);

        // Update loading text until scene is fully loaded
        while (!asyncLoad.isDone)
        {

            if (asyncLoad.progress >= 0.9f)
            {
                // Activate the new scene
                asyncLoad.allowSceneActivation = true;

                // Start fade out
                SetTime(6f, 0f);
                yield return StartCoroutine("Fade", "Out");
            }

            yield return null;
        }
    }
    //페이드 인 아웃
    private Image fadeImage;

    private float playTime;
    private float delayTime;
    private Color color;

    private void Awake()
    {
        DontDestroyOnLoad(transform.root.gameObject);
        fadeImage = gameObject.GetComponent<Image>();
        gameObject.SetActive(true);
        SetAlpha(0f);
    }

    public void SetTime(float playTime, float delayTime)
    {
        this.playTime = playTime;
        this.delayTime = delayTime;
    }

    private void SetAlpha(float alpha)
    {
        color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }

    public IEnumerator Fade(string property)
    {
        float t = 0f;
        float start, end;

        if (property == "In")
        {
            start = 0f;
            end = 1f;
            SetAlpha(0f);
        }

        else
        {
            start = 1f;
            end = 0f;
            SetAlpha(1f);
        }

        yield return new WaitForSeconds(delayTime);

        while (t < 1)
        {
            t += Time.deltaTime / playTime;
            SetAlpha(Mathf.Lerp(start, end, t));

            yield return null;
        }
    }
}