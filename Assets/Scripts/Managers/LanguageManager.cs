using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageManager : Singleton<LanguageManager>
{
    public enum Language
    {
        KO,
        EN,
        JA,
        ZH,
        ZH_TW
    }
    
    public Language currentLanguage;
    public event Action<Language> OnLanguageChanged;
    
    // 언어 전체 설정 / 변경 -------------------------------------
    private void SaveLanguage(Language language)
    {
        PlayerPrefs.SetInt("Language", (int)language);
        PlayerPrefs.Save();
    }

    private Language LoadLanguage()
    {
        Language language_enum;
        int value = PlayerPrefs.GetInt("Language", -1);

        if (value == -1)
        {
            string language_str = SteamManager.instance.RefreshSteamLanguage();
            switch (language_str)
            {
                case "koreana": language_enum = Language.KO; break;
                case "english": language_enum = Language.EN; break;
                case "japanese": language_enum = Language.JA; break;
                case "schinese": language_enum = Language.ZH; break;
                case "tchinese": language_enum = Language.ZH_TW; break;
                default: language_enum = Language.EN; break;
            }
        }

        else language_enum = (Language)value;
        
        return language_enum;
    }
    
    public void SetLanguage(Language language)
    {
        if (currentLanguage != language)
        {
            currentLanguage = language;
            SaveLanguage(language);
            OnLanguageChanged?.Invoke(language);
        }
    }
    
    protected override void Awake()
    {
        base.Awake();
        currentLanguage = LoadLanguage();
        OnLanguageChanged += ChangeFixedUIText;
    }

    private void OnDestroy()
    {
        OnLanguageChanged -= ChangeFixedUIText;
    }
    
    // fixed UI Text 변경 -------------------------------------
    private List<TMP_Text> fixedUITexts = new List<TMP_Text>();
    
    private bool GetFixedUIText()
    {
        fixedUITexts.Clear();
        GameObject[] fixedUIs = GameObject.FindGameObjectsWithTag("Fixed Text UI");
        if (fixedUIs.Length == 0) return false;

        foreach (var fixedUI in fixedUIs)
        {
            TMP_Text tmpText = fixedUI.GetComponent<TMP_Text>();
            if(tmpText != null) fixedUITexts.Add(tmpText);
        }

        return fixedUITexts.Count != 0;
    }

    public void ChangeFixedUIText(Language language)
    {
        if (!GetFixedUIText()) return;
        
        TextAsset fixedUITextAsset = Resources.Load<TextAsset>($"FixedSettings_{language}");
        if (fixedUITextAsset != null)
        {
            string[] loadedText = fixedUITextAsset.text.Split('\n');
            int index = 0;
            foreach (var UIText in fixedUITexts)
            {
                if (index < loadedText.Length)
                {
                    UIText.text = loadedText[index];
                    ++index;
                }

                else UIText.text = "No Data";
            }
        }
        
        else Debug.LogError($"FixedSettings_{language}.txt 파일을 찾을 수 없습니다!");
    }
    
    public string GetText(string key)
    {
        return key;
    }
}