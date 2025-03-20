using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private List<TMP_Text> fixedUITexts = new List<TMP_Text>();
    
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
            //string language_str = SteamManager.instance.RefreshSteamLanguage(); 플랫폼별 초기화
            string language_str = "koreana";
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
    
    protected override void Awake()
    {
        base.Awake();
        currentLanguage = LoadLanguage();
        GetFixedUIText(); //true면 파일 로드 후 글자 변경
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetFixedUIText();
    }

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
    
    public void SetLanguage(Language language)
    {
        if (currentLanguage != language)
        {
            currentLanguage = language;
            SaveLanguage(language);
            OnLanguageChanged?.Invoke(language);
        }
    }

    public string GetText(string key)
    {
        return key;
    }
}