using System;

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

    public void SetLanguage(Language language)
    {
        if (currentLanguage != language)
        {
            currentLanguage = language;
            OnLanguageChanged?.Invoke(language);
        }
    }

    public string GetText(string key)
    {
        return key;
    }
}