using Word.Domain.Constants;


namespace Word.Domain.Entities;

public class WordEntity
{
    public int Id { get; private set; }
    public string EnglishWord { get; private set; } = string.Empty;
    public string KyrgyzWord{ get; private set; } = string.Empty;
    public int CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public bool IsActive { get; private set; }

    public WordEntity(string englishWord, string kyrgyzWord, int categoryId)
    {
        if (englishWord.Length > DomainConstraints.EnglishWordMaxLength)
        {
            throw new Exception("Word EnglishWord is too long");
        }

        if (kyrgyzWord.Length > DomainConstraints.KyrgyzWordMaxLength)
        {
            throw new Exception("Word KyrgyzWord is too long");
        }

        if (categoryId <= 0)
        {
            throw new Exception("CategoryId must be greater than zero");
        }

        EnglishWord = englishWord;
        KyrgyzWord = kyrgyzWord;
        CategoryId = categoryId;
        IsActive = true;
    }
}
