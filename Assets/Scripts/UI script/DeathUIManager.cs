using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System;

public class DeathUIManager : MonoBehaviour
{
    public GameObject canvas;
    public TextMeshProUGUI messageText;
    public Image background;

    [SerializeField] private int deathMessageCount = 5;
    [SerializeField] private int teabaggingMessageCount = 10; //몇번째 죽음마다 출력할 건지
    [SerializeField] private float ShowedTime = 5.0f;
    [SerializeField] private float fadetime = 0.5f; // 사망 시 팁/티배깅 메시지 출력되는 시간
    [SerializeField] private int backGroundWidthBlank = 200;
    [SerializeField] private int backGroundHeightBlank = 30;
    
    private List<string> deathMessages = new List<string>();
    private List<string> teabaggingMessages = new List<string>();
    private bool isPlaying = false;
    
    private void Start()
    {
        GameManager.instance.onBalloonRespawn.AddListener(EnableDeathUI);
        LoadMessages(LanguageManager.instance.currentLanguage);
        LanguageManager.instance.OnLanguageChanged += LoadMessages;
        canvas.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.instance.onBalloonRespawn.RemoveListener(EnableDeathUI);
        LanguageManager.instance.OnLanguageChanged -= LoadMessages;
    }
    private void EnableDeathUI()
    {
        if (isPlaying) return;
        StartCoroutine(ShowDeathMessage());
    }
    
    private void LoadMessages(LanguageManager.Language language)
    {
        deathMessages.Clear();
        teabaggingMessages.Clear();

        TextAsset deathTextAsset = Resources.Load<TextAsset>($"deathMessages_{language}");
        TextAsset teabaggingTextAsset = Resources.Load<TextAsset>($"teabaggingMessages_{language}");
        if (deathTextAsset != null)
        {
            deathMessages.AddRange(deathTextAsset.text.Split('\n'));
        }
        else
        {
            Debug.LogError($"deathMessages_{language}.txt 파일을 찾을 수 없습니다!");
        }

        if (teabaggingTextAsset != null)
        {
            teabaggingMessages.AddRange(teabaggingTextAsset.text.Split('\n'));
        }
        else
        {
            Debug.LogError($"teabaggingMessages_{language}.txt 파일을 찾을 수 없습니다!");
        }
    }

    // n번째 사망할 때마다 티배깅용 멘트 출력
    // 멘트 출력시 각 txt파일에서 랜덤으로 멘트 선정
    private IEnumerator ShowDeathMessage()
    {
        isPlaying = true;
        canvas.gameObject.SetActive(true);
        
        if (deathMessages.Count > 0)
        {
            string randomMessage = "";
            if (SaveManager.instance.DeathCount % teabaggingMessageCount == 0)
            {
                randomMessage = GetRandomTeabaggingMessage();
            }
            
            else if (SaveManager.instance.DeathCount % deathMessageCount == 0)
            {
                randomMessage = GetRandomDeathMessage();
            }
            
            messageText.text = randomMessage;
            AdjustBackgroundSize(messageText, background);
            
            StartCoroutine(FadeInBackground(background));
            StartCoroutine(FadeInText(messageText));
        }
        yield return new WaitForSeconds(fadetime);
        yield return new WaitForSeconds(ShowedTime);
        
        StartCoroutine(FadeOut(background));
        StartCoroutine(FadeOut(messageText));
        yield return new WaitForSeconds(fadetime);

        canvas.gameObject.SetActive(false);
        isPlaying = false;
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
        backgroundRect.sizeDelta = new Vector2(textRect.rect.width + backGroundWidthBlank, textRect.rect.height + backGroundHeightBlank);
        background.pixelsPerUnitMultiplier = 1;
        background.fillCenter = true;

        Canvas.ForceUpdateCanvases();
    }
}