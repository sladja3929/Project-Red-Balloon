public class LanguageManager : Singleton<LanguageManager>
{
    public enum Language
    {
        English,
        Korean
    }

    public Language currentLanguage;

    public void SetLanguage(Language language)
    {
        currentLanguage = language;
    }

    public string GetText(string key)
    {
        return key;
    }
}