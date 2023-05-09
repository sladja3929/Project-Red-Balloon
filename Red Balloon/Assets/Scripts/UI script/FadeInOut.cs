using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class FadeInOut : MonoBehaviour

{
    [SerializeField] private Image fadeImage;
    [SerializeField] private bool isFadeIn;
    [SerializeField] private float playTime;
    [SerializeField] private float delayTime;
    private Color color;

    private void Awake()
    {
        //fadeImage.gameObject.SetActive(true);
        //SetAlpha(0f);
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

    private IEnumerator Fade(string property)
    {
        float t = 0f;
        float start, end;

        if(property == "In")
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

        while(t < 1)
        {
            t += Time.deltaTime / playTime;
            SetAlpha(Mathf.Lerp(start, end, t));

            yield return null;
        }
    }

}