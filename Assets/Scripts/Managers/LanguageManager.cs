using System;
using UnityEngine;

public class LanguageManager : Singleton<LanguageManager>
{
    public enum Language
    {
        KR,
        EN,
        JP,
        CHT
    }

    public Language currentLanguage;
    public event Action<Language> OnLanguageChanged;

    private void SaveLanguage(Language language)
    {
        PlayerPrefs.SetInt("Language", (int)language);
        PlayerPrefs.Save();
    }

    private Language LoadLanguage()
    {
        int value = PlayerPrefs.GetInt("Language", 0);
        return (Language)value;
    }
    
    protected override void Awake()
    {
        base.Awake();
        currentLanguage = LoadLanguage();
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