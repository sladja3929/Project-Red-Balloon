using System;
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