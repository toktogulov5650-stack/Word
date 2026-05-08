using Word.Domain.Constants;

namespace Word.Domain.Entities;

public class WordTranslation
{
    private WordTranslation()
    {
    }

    public WordTranslation(string languageCode, string text)
    {
        LanguageCode = LanguageCodes.NormalizeSupported(languageCode, nameof(languageCode));
        Text = NormalizeRequired(text, nameof(text), DomainConstraints.WordTranslationTextMaxLength);
    }

    public int Id { get; private set; }
    public int WordId { get; private set; }
    public string LanguageCode { get; private set; } = string.Empty;
    public string Text { get; private set; } = string.Empty;
    public WordEntity Word { get; private set; } = null!;

    public void UpdateText(string text)
    {
        Text = NormalizeRequired(text, nameof(text), DomainConstraints.WordTranslationTextMaxLength);
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
}
