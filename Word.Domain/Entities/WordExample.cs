namespace Word.Domain.Entities;

public class WordExample
{
    private readonly List<WordExampleTranslation> _translations = [];

    private WordExample()
    {
    }

    public WordExample(int sortOrder)
    {
        if (sortOrder <= 0)
            throw new ArgumentOutOfRangeException(nameof(sortOrder), "Sort order must be greater than zero.");

        SortOrder = sortOrder;
    }

    public int Id { get; private set; }
    public int WordExplanationId { get; private set; }
    public int SortOrder { get; private set; }
    public WordExplanation WordExplanation { get; private set; } = null!;
    public IReadOnlyCollection<WordExampleTranslation> Translations => _translations;

    public void AddOrUpdateTranslation(string languageCode, string text, string translation)
    {
        var normalizedLanguageCode = NormalizeRequired(languageCode, nameof(languageCode), 10).ToLowerInvariant();
        var existingTranslation = _translations.FirstOrDefault(x =>
            string.Equals(x.LanguageCode, normalizedLanguageCode, StringComparison.OrdinalIgnoreCase));

        if (existingTranslation is not null)
        {
            existingTranslation.Update(text, translation);
            return;
        }

        _translations.Add(new WordExampleTranslation(normalizedLanguageCode, text, translation));
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
