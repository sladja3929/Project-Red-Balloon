using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class DeathUIManager : MonoBehaviour
{
    public TextMeshProUGUI deathMessageText;
    public TextMeshProUGUI TeabaggingText;

    private List<string> deathMessages = new List<string>();
    private List<string> teabaggingMessages = new List<string>();
    [SerializeField] private int deathCount;
    [SerializeField] private float ShowedTime = 5.0f;
    [SerializeField] private float fadetime = 0.5f; // ��� �� ��/Ƽ��� �޽��� ��µǴ� �ð�
    [SerializeField] private float deleteDeathUITime = 6.0f;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    void OnEnable()
    {
        LoadDeathCount();
        LoadMessages();
        IncreaseDeathCount();
        ShowDeathMessage();
        Invoke("DeleteDeathUI", deleteDeathUITime);
    }

    private void OnDisable()
    {
        deathMessageText.gameObject.SetActive(false);
        TeabaggingText.gameObject.SetActive(false);
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
        deathCount = PlayerPrefs.GetInt("DeathCount", 0);
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
                StartCoroutine(FadeInText(TeabaggingText));
            }
            else
            {
                deathMessageText.gameObject.SetActive(true);
                string randomMessage = GetRandomDeathMessage();
                deathMessageText.text = randomMessage;
                StartCoroutine(FadeInText(deathMessageText));
            }
            Invoke("HideDeathMessage", ShowedTime);
        }
    }

    private void IncreaseDeathCount()
    {
        deathCount++;
        //SaveManager.instance.DeathCount = deathCount;
        //SaveManager.instance.Save();
        PlayerPrefs.SetInt("DeathCount", deathCount);
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
    }

    void DeleteDeathUI()
    {
        gameObject.SetActive(false);
    }
}
