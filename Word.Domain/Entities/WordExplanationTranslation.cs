using Word.Domain.Constants;

namespace Word.Domain.Entities;

public class WordExplanationTranslation
{
    private WordExplanationTranslation()
    {
    }

    public WordExplanationTranslation(
        string languageCode,
        string whatIs,
        string meaning,
        string translations,
        string usage,
        string hint)
    {
        LanguageCode = NormalizeRequired(languageCode, nameof(languageCode), DomainConstraints.LanguageCodeMaxLength)
            .ToLowerInvariant();
        WhatIs = NormalizeOptional(whatIs, DomainConstraints.WordExplanationWhatIsMaxLength);
        Meaning = NormalizeOptional(meaning, DomainConstraints.WordExplanationMeaningMaxLength);
        Translations = NormalizeOptional(translations, DomainConstraints.WordExplanationTranslationsMaxLength);
        Usage = NormalizeOptional(usage, DomainConstraints.WordExplanationUsageMaxLength);
        Hint = NormalizeOptional(hint, DomainConstraints.WordExplanationHintMaxLength);
    }

    public int Id { get; private set; }
    public int WordExplanationId { get; private set; }
    public string LanguageCode { get; private set; } = string.Empty;
    public string WhatIs { get; private set; } = string.Empty;
    public string Meaning { get; private set; } = string.Empty;
    public string Translations { get; private set; } = string.Empty;
    public string Usage { get; private set; } = string.Empty;
    public string Hint { get; private set; } = string.Empty;
    public WordExplanation WordExplanation { get; private set; } = null!;

    public void Update(
        string whatIs,
        string meaning,
        string translations,
        string usage,
        string hint)
    {
        WhatIs = NormalizeOptional(whatIs, DomainConstraints.WordExplanationWhatIsMaxLength);
        Meaning = NormalizeOptional(meaning, DomainConstraints.WordExplanationMeaningMaxLength);
        Translations = NormalizeOptional(translations, DomainConstraints.WordExplanationTranslationsMaxLength);
        Usage = NormalizeOptional(usage, DomainConstraints.WordExplanationUsageMaxLength);
        Hint = NormalizeOptional(hint, DomainConstraints.WordExplanationHintMaxLength);
    }

    private static string NormalizeRequired(string value, string paramName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} is required.", paramName);

        var normalizedValue = value.Trim();

        if (normalizedValue.Length > maxLength)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} must be at most {maxLength} characters long.");

        return normalizedValue;
    }

    private static string NormalizeOptional(string value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var normalizedValue = value.Trim();

        if (normalizedValue.Length > maxLength)
            throw new ArgumentOutOfRangeException(nameof(value), $"value must be at most {maxLength} characters long.");

        return normalizedValue;
    }
}
