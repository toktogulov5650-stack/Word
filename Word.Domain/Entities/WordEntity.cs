using Word.Domain.Constants;

namespace Word.Domain.Entities;

public class WordEntity
{
    private readonly List<WordTranslation> _wordTranslations = [];

    private WordEntity()
    {
    }

    public int Id { get; private set; }
    public string EnglishWord { get; private set; } = string.Empty;
    public IReadOnlyCollection<WordTranslation> WordTranslations => _wordTranslations;
    public int CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public WordExplanation? Explanation { get; private set; }

    public WordEntity(string englishWord, int categoryId)
    {
        englishWord = NormalizeRequired(englishWord, nameof(englishWord));

        if (englishWord.Length > DomainConstraints.EnglishWordMaxLength)
        {
            throw new Exception("Word EnglishWord is too long");
        }

        if (categoryId <= 0)
        {
            throw new Exception("CategoryId must be greater than zero");
        }

        EnglishWord = englishWord;
        CategoryId = categoryId;
        IsActive = true;
    }

    public WordEntity(string englishWord, string kyrgyzWord, int categoryId)
        : this(englishWord, categoryId)
    {
        AddTranslation(kyrgyzWord);
    }

    public void AddTranslation(string kyrgyzWord)
    {
        var normalizedTranslation = NormalizeRequired(kyrgyzWord, nameof(kyrgyzWord));

        if (_wordTranslations.Any(x =>
                string.Equals(x.KyrgyzWord, normalizedTranslation, StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        _wordTranslations.Add(new WordTranslation(normalizedTranslation));
    }

    private static string NormalizeRequired(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{paramName} is required.", paramName);
        }

        return value.Trim();
    }
}
