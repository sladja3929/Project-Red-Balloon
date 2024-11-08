using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class DeathUIManager : MonoBehaviour
{
    public Text deathMessageText;
    public Text TeabaggingText;
    public Text deathCountText; 

    private List<string> deathMessages = new List<string>();
    private List<string> teabaggingMessages = new List<string>();
    [SerializeField]private int deathCount;
    public float fadetime = 2.0f;//사망 시 팁/티배깅 메시지 출력되는 시간

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

    private void LoadDeathCount()
    {
        deathCount = PlayerPrefs.GetInt("death_count", 0);
    }

    //5*n번째 사망할 때마다 티배깅용 멘트 출력
    //멘트 출력시 각 txt파일에서 랜덤으로 멘트 선정
    public void ShowDeathMessage()
    {
        if (deathMessages.Count > 0)
        {
            if(deathCount % 5 == 0)
            {
                TeabaggingText.gameObject.SetActive(true);
                string randomMessage = GetRandomTeabaggingMessage();
                TeabaggingText.text = randomMessage;
                StartCoroutine(FadeIn(TeabaggingText));
            }
            else
            {
                deathMessageText.gameObject.SetActive(true);
                string randomMessage = GetRandomDeathMessage();
                deathMessageText.text = randomMessage;
                StartCoroutine(FadeIn(deathMessageText));
            }
        }
    }

    private void ShowDeathCount()
    {
        IncreaseDeathCount();
        deathCountText.text = $"{deathCount}개째 풍선";
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

    private IEnumerator FadeIn(Text message)
    {
        Text messageText = message;
        Color color = messageText.color;
        color.a = 0;
        messageText.color = color; 
        float elapsedTime = 0f;
        while (elapsedTime < fadetime)
        { 
            elapsedTime += Time.deltaTime; color.a = Mathf.Clamp01(elapsedTime / fadetime);
            messageText.color = color;
            yield return null;
        }
        color.a = 1;
        messageText.color = color;
    }
}
