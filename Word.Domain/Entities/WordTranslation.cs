using Word.Domain.Constants;

namespace Word.Domain.Entities;

public class WordTranslation
{
    private WordTranslation()
    {
    }

    public WordTranslation(string kyrgyzWord)
    {
        kyrgyzWord = NormalizeRequired(kyrgyzWord, nameof(kyrgyzWord));

        if (kyrgyzWord.Length > DomainConstraints.KyrgyzWordMaxLength)
        {
            throw new Exception("Word KyrgyzWord is too long");
        }

        KyrgyzWord = kyrgyzWord;
    }

    public int Id { get; private set; }
    public int WordId { get; private set; }
    public string KyrgyzWord { get; private set; } = string.Empty;
    public WordEntity Word { get; private set; } = null!;

    private static string NormalizeRequired(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{paramName} is required.", paramName);
        }

        return value.Trim();
    }
}
