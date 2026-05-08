using Word.Domain.Constants;

namespace Word.Domain.Entities;

public class WordExplanation
{
    private readonly List<WordExplanationTranslation> _translations = [];
    private readonly List<WordExample> _examples = [];

    private WordExplanation()
    {
    }

    public WordExplanation(int wordId)
    {
        if (wordId <= 0)
            throw new ArgumentOutOfRangeException(nameof(wordId), "Word ID must be greater than zero.");

        WordId = wordId;
    }

    public int Id { get; private set; }
    public int WordId { get; private set; }
    public WordEntity Word { get; private set; } = null!;
    public IReadOnlyCollection<WordExplanationTranslation> Translations => _translations;
    public IReadOnlyCollection<WordExample> Examples => _examples;

    public void AddOrUpdateTranslation(
        string languageCode,
        string whatIs,
        string meaning,
        string translations,
        string usage,
        string hint)
    {
        var normalizedLanguageCode = NormalizeRequired(
            languageCode,
            nameof(languageCode),
            DomainConstraints.LanguageCodeMaxLength).ToLowerInvariant();

        var normalizedWhatIs = NormalizeOptional(whatIs, DomainConstraints.WordExplanationWhatIsMaxLength);
        var normalizedMeaning = NormalizeOptional(meaning, DomainConstraints.WordExplanationMeaningMaxLength);
        var normalizedTranslations = NormalizeOptional(translations, DomainConstraints.WordExplanationTranslationsMaxLength);
        var normalizedUsage = NormalizeOptional(usage, DomainConstraints.WordExplanationUsageMaxLength);
        var normalizedHint = NormalizeOptional(hint, DomainConstraints.WordExplanationHintMaxLength);

        var existingTranslation = _translations.FirstOrDefault(x =>
            string.Equals(x.LanguageCode, normalizedLanguageCode, StringComparison.OrdinalIgnoreCase));

        if (existingTranslation is not null)
        {
            existingTranslation.Update(
                normalizedWhatIs,
                normalizedMeaning,
                normalizedTranslations,
                normalizedUsage,
                normalizedHint);
            return;
        }

        _translations.Add(new WordExplanationTranslation(
            normalizedLanguageCode,
            normalizedWhatIs,
            normalizedMeaning,
            normalizedTranslations,
            normalizedUsage,
            normalizedHint));
    }

    public void AddOrUpdateExample(
        int sortOrder,
        string languageCode,
        string text,
        string translation)
    {
        if (sortOrder <= 0)
            throw new ArgumentOutOfRangeException(nameof(sortOrder), "Sort order must be greater than zero.");

        var example = _examples.FirstOrDefault(x => x.SortOrder == sortOrder);

        if (example is null)
        {
            example = new WordExample(sortOrder);
            _examples.Add(example);
        }

        example.AddOrUpdateTranslation(languageCode, text, translation);
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
