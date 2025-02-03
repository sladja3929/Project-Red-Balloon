using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System;

public class DeathUIManager : MonoBehaviour
{
    public GameObject canvas;
    public TextMeshProUGUI deathMessageText;
    public TextMeshProUGUI TeabaggingText;
    public Image background;

    [SerializeField] private int deathMessageCount = 5;
    [SerializeField] private int teabaggingMessageCount = 10; //몇번째 죽음마다 출력할 건지
    [SerializeField] private float ShowedTime = 5.0f;
    [SerializeField] private float fadetime = 0.5f; // 사망 시 팁/티배깅 메시지 출력되는 시간
    [SerializeField] private float deleteDeathUITime = 6.0f;
    
    private List<string> deathMessages = new List<string>();
    private List<string> teabaggingMessages = new List<string>();

    private void Start()
    {
        LoadMessages();
        GameManager.instance.onBalloonRespawn.AddListener(EnableDeathUI);
        canvas.gameObject.SetActive(false);
    }
    private void EnableDeathUI()
    {
        canvas.gameObject.SetActive(true);
        ShowDeathMessage();
        Invoke("DeleteDeathUI", deleteDeathUITime);
    }

    private void DisableDeathUI()
    {
        canvas.gameObject.SetActive(false);
        deathMessageText.gameObject.SetActive(false);
        TeabaggingText.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
    }
    
    private void LoadMessages()
    {
        TextAsset deathTextAsset = Resources.Load<TextAsset>("deathMessages");
        TextAsset teabaggingTextAsset = Resources.Load<TextAsset>("teabaggingMessages");
        if (deathTextAsset != null)
        {
            deathMessages.AddRange(deathTextAsset.text.Split('\n'));
        }
        else
        {
            Debug.LogError("deathMessages.txt 파일을 찾을 수 없습니다!");
        }

        if (teabaggingTextAsset != null)
        {
            teabaggingMessages.AddRange(teabaggingTextAsset.text.Split('\n'));
        }
        else
        {
            Debug.LogError("teabaggingMessages.txt 파일을 찾을 수 없습니다!");
        }
    }

    // n번째 사망할 때마다 티배깅용 멘트 출력
    // 멘트 출력시 각 txt파일에서 랜덤으로 멘트 선정
    public void ShowDeathMessage()
    {
        if (deathMessages.Count > 0)
        {
            if (SaveManager.instance.DeathCount % teabaggingMessageCount == 0)
            {
                TeabaggingText.gameObject.SetActive(true);
                string randomMessage = GetRandomTeabaggingMessage();
                TeabaggingText.text = randomMessage;
                background.gameObject.SetActive(true);
                AdjustBackgroundSize(TeabaggingText, background);
                StartCoroutine(FadeInBackground(background));
                StartCoroutine(FadeInText(TeabaggingText));
            }
            else if (SaveManager.instance.DeathCount % deathMessageCount == 0)
            {
                deathMessageText.gameObject.SetActive(true);
                string randomMessage = GetRandomDeathMessage();
                deathMessageText.text = randomMessage;
                background.gameObject.SetActive(true);
                AdjustBackgroundSize(deathMessageText, background);
                StartCoroutine(FadeInBackground(background));
                StartCoroutine(FadeInText(deathMessageText));
            }
            Invoke("HideDeathMessage", ShowedTime);
        }
    }

    private string GetRandomDeathMessage()
    {
        int randomIndex = UnityEngine.Random.Range(0, deathMessages.Count);
        return deathMessages[randomIndex].Trim();
    }

    private string GetRandomTeabaggingMessage()
    {
        int randomIndex = UnityEngine.Random.Range(0, teabaggingMessages.Count);
        return teabaggingMessages[randomIndex].Trim();
    }

    private IEnumerator FadeInText(Graphic graphic)
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

    private void HideDeathMessage()
    {
        if (SaveManager.instance.DeathCount % teabaggingMessageCount == 0)
        {
            StartCoroutine(FadeOut(TeabaggingText));
        }
        else if (SaveManager.instance.DeathCount % deathMessageCount == 0)
        {
            StartCoroutine(FadeOut(deathMessageText));
        }
        StartCoroutine(FadeOut(background));
    }

    void DeleteDeathUI()
    {
        canvas.gameObject.SetActive(false);
    }

    private IEnumerator FadeInBackground(Image background)
    {
        Color color = background.color;
        color.a = 0.5f; // 반투명하게 설정
        background.color = color;
        float elapsedTime = 0f;
        while (elapsedTime < fadetime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadetime * 0.5f); // 반투명하게 페이드 인
            background.color = color;
            yield return null;
        }
        color.a = 0.5f;
        background.color = color;
    }
    private void AdjustBackgroundSize(TextMeshProUGUI text, Image background)
    {
        RectTransform textRect = text.GetComponent<RectTransform>();
        RectTransform backgroundRect = background.GetComponent<RectTransform>();

        ContentSizeFitter fitter = text.gameObject.GetComponent<ContentSizeFitter>();
        if (fitter == null)
        {
            fitter = text.gameObject.AddComponent<ContentSizeFitter>();
        }
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        LayoutElement layoutElement = text.gameObject.GetComponent<LayoutElement>();
        if (layoutElement == null)
        {
            layoutElement = text.gameObject.AddComponent<LayoutElement>();
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(textRect);
        backgroundRect.sizeDelta = new Vector2(textRect.rect.width + 500, textRect.rect.height + 120);
        background.pixelsPerUnitMultiplier = 1;
        background.fillCenter = true;

        Canvas.ForceUpdateCanvases();
    }

    private void OnDestroy()
    {
        GameManager.instance.onBalloonRespawn.RemoveListener(EnableDeathUI);
    }
}

