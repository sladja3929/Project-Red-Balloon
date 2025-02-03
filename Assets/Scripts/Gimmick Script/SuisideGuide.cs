using UnityEngine;

public class SuisideGuide : Gimmick
{
    public GameObject text;
    
    public override void Execute()
    {
        // coroutine
        // StartCoroutine(FadeTextCoroutine());
    }
    
    // text를 천천히 fade in 또는 fade out 하는 함수
    public void FadeText(bool isFadeIn)
    {
        if (isFadeIn)
        {
            text.SetActive(true);
            text.GetComponent<CanvasGroup>().alpha += Time.deltaTime;
        }
        else
        {
            text.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
            if (text.GetComponent<CanvasGroup>().alpha <= 0)
            {
                text.SetActive(false);
            }
        }
    }
}