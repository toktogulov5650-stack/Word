namespace Word.Domain.Constants;

public static class LanguageCodes
{
    public const string Russian = "ru";
    public const string Kyrgyz = "ky";
    public const string Default = Russian;

    public static string NormalizeOrDefault(string? languageCode, string paramName = "languageCode")
    {
        if (string.IsNullOrWhiteSpace(languageCode))
            return Default;

        return NormalizeSupported(languageCode, paramName);
    }

    public static string NormalizeSupported(string languageCode, string paramName = "languageCode")
    {
        if (string.IsNullOrWhiteSpace(languageCode))
            throw new ArgumentException("Language code is required.", paramName);

        var normalized = languageCode.Trim().ToLowerInvariant();

        return normalized switch
        {
            Kyrgyz => Kyrgyz,
            Russian => Russian,
            _ => throw new ArgumentException("Language code must be 'ky' or 'ru'.", paramName)
        };
    }
}
