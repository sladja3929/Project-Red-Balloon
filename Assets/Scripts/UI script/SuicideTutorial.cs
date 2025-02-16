using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuicideTutorial : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image image;
    public float fadetime;

    private bool isPlay = false;

    private void Awake()
    {
        Color color = image.color;
        color.a = 0;
        image.color = color;
        
        color = text.color;
        color.a = 0;
        text.color = color;
    }

    private void Start()
    {
        if (SaveManager.instance.DeathCount >= 5) Destroy(gameObject);

        else
        {
            GameManager.instance.onBalloonRespawn.AddListener(Show);
        }
    }

    private void Show()
    {
        if (!isPlay && SaveManager.instance.DeathCount == 5)
        {
            GameManager.instance.onBalloonRespawn.RemoveListener(Show);
            StartCoroutine(ShowCoroutine());
        }
        
    }
    
    private IEnumerator ShowCoroutine()
    {
        isPlay = true;
        
        StartCoroutine(FadeIn(text));
        yield return StartCoroutine(FadeIn(image));

        yield return new WaitForSeconds(5f);
        
        StartCoroutine(FadeOut(text));
        yield return StartCoroutine(FadeOut(image));
        
        Destroy(gameObject);
    }
    
    private IEnumerator FadeIn(Graphic graphic)
    {
        Color color = graphic.color;
        color.a = 0;
        graphic.color = color;
        float elapsedTime = 0f;
        while (elapsedTime < fadetime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadetime);
            graphic.color = color; 
            yield return null;
        }
        color.a = 1;
        graphic.color = color;
    }

    private IEnumerator FadeOut(Graphic graphic)
    {
        Color color = graphic.color;
        float startAlpha = color.a; // 현재 투명도 상태 저장
        float elapsedTime = 0f;
        while (elapsedTime < fadetime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, 0, elapsedTime / fadetime); // 현재 투명도에서 0으로 페이드 아웃
            graphic.color = color;
            yield return null;
        }
        color.a = 0;
        graphic.color = color;
    }
}
