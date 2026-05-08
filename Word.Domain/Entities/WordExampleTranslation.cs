using Word.Domain.Constants;

namespace Word.Domain.Entities;

public class WordExampleTranslation
{
    private WordExampleTranslation()
    {
    }

    public WordExampleTranslation(string languageCode, string text, string translation)
    {
        LanguageCode = LanguageCodes.NormalizeSupported(languageCode, nameof(languageCode));
        Text = NormalizeOptional(text, DomainConstraints.WordExplanationExampleMaxLength);
        Translation = NormalizeOptional(translation, DomainConstraints.WordExplanationExampleMaxLength);
    }

    public int Id { get; private set; }
    public int WordExampleId { get; private set; }
    public string LanguageCode { get; private set; } = string.Empty;
    public string Text { get; private set; } = string.Empty;
    public string Translation { get; private set; } = string.Empty;
    public WordExample WordExample { get; private set; } = null!;

    public void Update(string text, string translation)
    {
        Text = NormalizeOptional(text, DomainConstraints.WordExplanationExampleMaxLength);
        Translation = NormalizeOptional(translation, DomainConstraints.WordExplanationExampleMaxLength);
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
