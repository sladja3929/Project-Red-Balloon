using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageController : MonoBehaviour
{
    // 언어 설정
    public Button leftButton;
    public Button rightButton;
    public TextMeshProUGUI languageText;

    private LanguageManager.Language[] languages = (LanguageManager.Language[])System.Enum.GetValues(typeof(LanguageManager.Language));
    private int currentLanguageIndex;

    private void Start()
    {
        Debug.Log("LanguageController Start");
        currentLanguageIndex = (int)LanguageManager.instance.currentLanguage;
        UpdateLanguageText();

        leftButton.onClick.AddListener(() => ChangeLanguage(1));
        rightButton.onClick.AddListener(() => ChangeLanguage(-1));

    }

    private void ChangeLanguage(int direction)
    {
        currentLanguageIndex = (currentLanguageIndex + direction + languages.Length) % languages.Length;
        LanguageManager.instance.SetLanguage(languages[currentLanguageIndex]);
        UpdateLanguageText();
    }

    private void UpdateLanguageText()
    {
        languageText.text = languages[currentLanguageIndex].ToString();
        switch(languages[currentLanguageIndex])
        {
            case LanguageManager.Language.KR:
                languageText.text = "Korean";
                break;
            case LanguageManager.Language.EN:
                languageText.text = "English";
                break;
            case LanguageManager.Language.JP:
                languageText.text = "Japanese";
                break;
            case LanguageManager.Language.CHT:
                languageText.text = "Chinese";
                break;
        }
        Debug.Log(languages[currentLanguageIndex].ToString());
    }

}
