using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class DeathUIManager : MonoBehaviour
{
    public Text deathMessageText;
    public Text TeabaggingText;
    public Text deathCountText;
    public RawImage balloonImage; // �̹��� ��������Ʈ�� ���� Image ������Ʈ

    private List<string> deathMessages = new List<string>();
    private List<string> teabaggingMessages = new List<string>();
    [SerializeField] private int deathCount;
    [SerializeField] private float ShowedTime = 5.0f;
    [SerializeField] private float fadetime = 1.0f; // ��� �� ��/Ƽ��� �޽��� ��µǴ� �ð�

    void OnEnable()
    {
        LoadDeathCount();
        LoadMessages();
        ShowDeathCount();
        ShowDeathMessage();
    }

    private void OnDisable()
    {
        deathMessageText.gameObject.SetActive(false);
        TeabaggingText.gameObject.SetActive(false);
        deathCountText.gameObject.SetActive(false);
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
                StartCoroutine(FadeIn(TeabaggingText));
            }
            else
            {
                deathMessageText.gameObject.SetActive(true);
                balloonImage.gameObject.SetActive(true); // �̹��� ��������Ʈ Ȱ��ȭ
                string randomMessage = GetRandomDeathMessage();
                deathMessageText.text = randomMessage;
                StartCoroutine(FadeIn(deathMessageText));
                StartCoroutine(FadeIn(balloonImage)); // �̹��� ��������Ʈ ���̵� ��
            }
            Invoke("HideDeathMessage", ShowedTime);
        }
    }

    private void ShowDeathCount()
    {
        deathCountText.gameObject.SetActive(true);
        IncreaseDeathCount();
        deathCountText.text = $"X{deathCount}";
        Invoke("HideDeathCount", ShowedTime);
    }

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
        color.a = 1;
        graphic.color = color;
        float elapsedTime = 0f;
        while (elapsedTime < fadetime)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1 - Mathf.Clamp01(elapsedTime / fadetime);
            graphic.color = color;
            yield return null;
        }
        color.a = 0;
        graphic.color = color;
    }

    private void HideDeathCount()
    {
        StartCoroutine(FadeOut(deathCountText));
        StartCoroutine(FadeOut(balloonImage)); // �̹��� ��������Ʈ ���̵� �ƿ�
    }

    private void HideDeathMessage()
    {
        if (deathCount % 5 == 0)
        {
            StartCoroutine(FadeOut(TeabaggingText));
        }
        else
        {
            StartCoroutine(FadeOut(deathMessageText));        }
    }
}
