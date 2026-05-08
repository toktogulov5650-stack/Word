using Word.Domain.Constants;

namespace Word.Domain.Entities;

public class CategoryTranslation
{
    private CategoryTranslation()
    {
    }

    public CategoryTranslation(string languageCode, string name, string description)
    {
        LanguageCode = NormalizeRequired(languageCode, nameof(languageCode), DomainConstraints.LanguageCodeMaxLength)
            .ToLowerInvariant();
        Name = NormalizeRequired(name, nameof(name), DomainConstraints.CategoryNameMaxLength);
        Description = NormalizeRequired(description, nameof(description), DomainConstraints.CategoryDescriptionMaxLength);
    }

    public int Id { get; private set; }
    public int CategoryId { get; private set; }
    public string LanguageCode { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Category Category { get; private set; } = null!;

    public void Update(string name, string description)
    {
        Name = NormalizeRequired(name, nameof(name), DomainConstraints.CategoryNameMaxLength);
        Description = NormalizeRequired(description, nameof(description), DomainConstraints.CategoryDescriptionMaxLength);
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
