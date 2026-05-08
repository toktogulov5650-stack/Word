using Word.Domain.Constants;

namespace Word.Domain.Entities;

public class WordEntity
{
    private readonly List<WordTranslation> _wordTranslations = [];

    private WordEntity()
    {
    }

    public WordEntity(string englishWord, int categoryId)
    {
        EnglishWord = NormalizeRequired(englishWord, nameof(englishWord), DomainConstraints.EnglishWordMaxLength);

        if (categoryId <= 0)
            throw new ArgumentOutOfRangeException(nameof(categoryId), "Category ID must be greater than zero.");

        CategoryId = categoryId;
        IsActive = true;
    }

    public WordEntity(string englishWord, string languageCode, string translation, int categoryId)
        : this(englishWord, categoryId)
    {
        AddOrUpdateTranslation(languageCode, translation);
    }

    public int Id { get; private set; }
    public string EnglishWord { get; private set; } = string.Empty;
    public IReadOnlyCollection<WordTranslation> WordTranslations => _wordTranslations;
    public int CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public WordExplanation? Explanation { get; private set; }

    public void AddOrUpdateTranslation(string languageCode, string text)
    {
        var normalizedLanguageCode = NormalizeRequired(
            languageCode,
            nameof(languageCode),
            DomainConstraints.LanguageCodeMaxLength).ToLowerInvariant();

        var normalizedText = NormalizeRequired(
            text,
            nameof(text),
            DomainConstraints.WordTranslationTextMaxLength);

        if (_wordTranslations.Any(x =>
                string.Equals(x.LanguageCode, normalizedLanguageCode, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(x.Text, normalizedText, StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        var existingTranslation = _wordTranslations.FirstOrDefault(x =>
            string.Equals(x.LanguageCode, normalizedLanguageCode, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(x.Text, normalizedText, StringComparison.OrdinalIgnoreCase));

        if (existingTranslation is not null)
        {
            existingTranslation.UpdateText(normalizedText);
            return;
        }

        _wordTranslations.Add(new WordTranslation(normalizedLanguageCode, normalizedText));
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
