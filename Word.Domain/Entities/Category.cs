using Word.Domain.Constants;

namespace Word.Domain.Entities;

public class Category
{
    private readonly List<WordEntity> _words = [];
    private readonly List<CategoryTranslation> _translations = [];

    private Category()
    {
    }

    public Category(string? imageUrl = null)
    {
        ImageUrl = NormalizeOptional(imageUrl, DomainConstraints.CategoryImageUrlMaxLength);
        IsActive = true;
    }

    public int Id { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public IReadOnlyCollection<WordEntity> Words => _words;
    public IReadOnlyCollection<CategoryTranslation> Translations => _translations;

    public void UpdateImageUrl(string? imageUrl)
    {
        ImageUrl = NormalizeOptional(imageUrl, DomainConstraints.CategoryImageUrlMaxLength);
    }

    public void AddOrUpdateTranslation(string languageCode, string name, string description)
    {
        var normalizedLanguageCode = NormalizeRequired(
            languageCode,
            nameof(languageCode),
            DomainConstraints.LanguageCodeMaxLength).ToLowerInvariant();

        var normalizedName = NormalizeRequired(
            name,
            nameof(name),
            DomainConstraints.CategoryNameMaxLength);

        var normalizedDescription = NormalizeRequired(
            description,
            nameof(description),
            DomainConstraints.CategoryDescriptionMaxLength);

        var existingTranslation = _translations.FirstOrDefault(x =>
            string.Equals(x.LanguageCode, normalizedLanguageCode, StringComparison.OrdinalIgnoreCase));

        if (existingTranslation is not null)
        {
            existingTranslation.Update(normalizedName, normalizedDescription);
            return;
        }

        _translations.Add(new CategoryTranslation(normalizedLanguageCode, normalizedName, normalizedDescription));
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

    private static string NormalizeOptional(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var normalizedValue = value.Trim();

        if (normalizedValue.Length > maxLength)
            throw new ArgumentOutOfRangeException(nameof(value), $"value must be at most {maxLength} characters long.");

        return normalizedValue;
    }
}
