using Word.Domain.Constants;

namespace Word.Application.Localization;

public static class LocalizedContentResolver
{
    public const string DefaultLanguageCode = LanguageCodes.Default;
    public const string SecondaryLanguageCode = LanguageCodes.Kyrgyz;

    public static string NormalizeRequestedLanguage(string? languageCode)
    {
        return LanguageCodes.NormalizeOrDefault(languageCode);
    }

    public static T? ResolveTranslation<T>(
        IEnumerable<T> translations,
        string? languageCode,
        Func<T, string> languageSelector)
        where T : class
    {
        var translationList = translations.ToList();

        foreach (var fallbackLanguage in BuildLanguageFallback(languageCode))
        {
            var match = translationList.FirstOrDefault(x =>
                string.Equals(languageSelector(x), fallbackLanguage, StringComparison.OrdinalIgnoreCase));

            if (match is not null)
                return match;
        }

        return translationList.FirstOrDefault();
    }

    public static IReadOnlyCollection<T> ResolveTranslations<T>(
        IEnumerable<T> translations,
        string? languageCode,
        Func<T, string> languageSelector)
        where T : class
    {
        var translationList = translations.ToList();

        foreach (var fallbackLanguage in BuildLanguageFallback(languageCode))
        {
            var matches = translationList
                .Where(x => string.Equals(languageSelector(x), fallbackLanguage, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matches.Count > 0)
                return matches;
        }

        return translationList;
    }

    private static IReadOnlyCollection<string> BuildLanguageFallback(string? languageCode)
    {
        var normalizedLanguageCode = NormalizeRequestedLanguage(languageCode);
        var fallbackOrder = new List<string> { normalizedLanguageCode };

        if (!fallbackOrder.Contains(DefaultLanguageCode, StringComparer.OrdinalIgnoreCase))
            fallbackOrder.Add(DefaultLanguageCode);

        if (!fallbackOrder.Contains(SecondaryLanguageCode, StringComparer.OrdinalIgnoreCase))
            fallbackOrder.Add(SecondaryLanguageCode);

        return fallbackOrder;
    }
}
