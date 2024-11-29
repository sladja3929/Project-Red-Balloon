using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class DeathUIManager : MonoBehaviour
{
    public TextMeshProUGUI deathMessageText;
    public TextMeshProUGUI TeabaggingText;
    //public TextMeshProUGUI deathCountText;
    //public Image TextBackground; // ��� �̹��� �߰�

    private List<string> deathMessages = new List<string>();
    private List<string> teabaggingMessages = new List<string>();
    [SerializeField] private int deathCount;
    [SerializeField] private float ShowedTime = 5.0f;
    [SerializeField] private float fadetime = 0.5f; // ��� �� ��/Ƽ��� �޽��� ��µǴ� �ð�
    //[SerializeField] private float fadeInImage = 0.2f; // ��� �̹��� ���̵� �� �ð�

    private void Start()
    {
        //TextBackground.type = Image.Type.Sliced;
    }
    void OnEnable()
    {
        LoadDeathCount();
        LoadMessages();
        //ShowDeathCount();
        IncreaseDeathCount();
        ShowDeathMessage();
    }

    private void OnDisable()
    {
        deathMessageText.gameObject.SetActive(false);
        TeabaggingText.gameObject.SetActive(false);
        //deathCountText.gameObject.SetActive(false);
        //TextBackground.gameObject.SetActive(false);
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
            Debug.LogError("deathMessages.txt ������ ã�� �� �����ϴ�!");
        }

        if (teabaggingTextAsset != null)
        {
            teabaggingMessages.AddRange(teabaggingTextAsset.text.Split('\n'));
        }
        else
        {
            Debug.LogError("teabaggingMessages.txt ������ ã�� �� �����ϴ�!");
        }
    }
    // ���� Ƚ�� �� �ҷ����� ��� ���� ���� �ʿ�
    private void LoadDeathCount()
    {
        deathCount = PlayerPrefs.GetInt("death_count", 0);
    }

    // 5*n��° ����� ������ Ƽ���� ��Ʈ ���
    // ��Ʈ ��½� �� txt���Ͽ��� �������� ��Ʈ ����
    public void ShowDeathMessage()
    {
        if (deathMessages.Count > 0)
        {
            if (deathCount % 5 == 0)
            {
                TeabaggingText.gameObject.SetActive(true);
                string randomMessage = GetRandomTeabaggingMessage();
                TeabaggingText.text = randomMessage;
                //TextBackground.gameObject.SetActive(true);
                //AdjustBackgroundSize(TeabaggingText, TextBackground);
                //StartCoroutine(FadeInBackground(TextBackground));
                StartCoroutine(FadeInText(TeabaggingText));
            }
            else
            {
                deathMessageText.gameObject.SetActive(true);
                string randomMessage = GetRandomDeathMessage();
                deathMessageText.text = randomMessage;
                //TextBackground.gameObject.SetActive(true);
                //AdjustBackgroundSize(deathMessageText, TextBackground);
                //StartCoroutine(FadeInBackground(TextBackground));
                StartCoroutine(FadeInText(deathMessageText));
            }
            Invoke("HideDeathMessage", ShowedTime);
        }
    }

    //private void ShowDeathCount()
    //{
    //    deathCountText.gameObject.SetActive(true);
    //    IncreaseDeathCount();
    //    deathCountText.text = $"<sprite=0> �� {deathCount}";
    //    StartCoroutine(FadeIn(deathCountText));
    //    Invoke("HideDeathCount", ShowedTime);
    //}

    private void IncreaseDeathCount()
    {
        deathCount++;
        PlayerPrefs.SetInt("death_count", deathCount);
        PlayerPrefs.Save();
    }

    private string GetRandomDeathMessage()
    {
        int randomIndex = Random.Range(0, deathMessages.Count);
        return deathMessages[randomIndex].Trim();
    }

    private string GetRandomTeabaggingMessage()
    {
        int randomIndex = Random.Range(0, teabaggingMessages.Count);
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
    //private IEnumerator FadeInBackground(Image background)
    //{
    //    Color color = background.color;
    //    color.a = 0.5f; // �������ϰ� ����
    //    background.color = color;
    //    float elapsedTime = 0f;
    //    while (elapsedTime < fadeInImage)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        color.a = Mathf.Clamp01(elapsedTime / fadeInImage * 0.5f); // �������ϰ� ���̵� ��
    //        background.color = color;
    //        yield return null;
    //    }
    //    color.a = 0.5f;
    //    background.color = color;
    //}

    private IEnumerator FadeOut(Graphic graphic)
    {
        Color color = graphic.color;
        float startAlpha = color.a; // ���� ���� ���� ����
        float elapsedTime = 0f;
        while (elapsedTime < fadetime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, 0, elapsedTime / fadetime); // ���� �������� 0���� ���̵� �ƿ�
            graphic.color = color;
            yield return null;
        }
        color.a = 0;
        graphic.color = color;
    }

    //private void HideDeathCount()
    //{
    //    StartCoroutine(FadeOut(deathCountText));
    //}

    private void HideDeathMessage()
    {
        if (deathCount % 5 == 0)
        {
            StartCoroutine(FadeOut(TeabaggingText));
        }
        else
        {
            StartCoroutine(FadeOut(deathMessageText));
        }
            //StartCoroutine(FadeOut(TextBackground));
    }
    //private void AdjustBackgroundSize(TextMeshProUGUI text, Image background)
    //{
    //    RectTransform textRect = text.GetComponent<RectTransform>();
    //    RectTransform backgroundRect = background.GetComponent<RectTransform>();

    //    ContentSizeFitter fitter = text.gameObject.GetComponent<ContentSizeFitter>();
    //    if (fitter == null)
    //    {
    //        fitter = text.gameObject.AddComponent<ContentSizeFitter>();
    //    }
    //    fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
    //    fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

    //    LayoutElement layoutElement = text.gameObject.GetComponent<LayoutElement>();
    //    if (layoutElement == null)
    //    {
    //        layoutElement = text.gameObject.AddComponent<LayoutElement>();
    //    }

    //    // �ؽ�Ʈ ũ�⿡ �°� ��� ũ�� ����
    //    LayoutRebuilder.ForceRebuildLayoutImmediate(textRect);
    //    backgroundRect.sizeDelta = new Vector2(textRect.rect.width + 50, textRect.rect.height + 30);
    //    background.pixelsPerUnitMultiplier = 1;
    //    background.fillCenter = true;
    //}
}
