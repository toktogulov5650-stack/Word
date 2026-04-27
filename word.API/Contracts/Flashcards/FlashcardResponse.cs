namespace Word.API.Contracts.Flashcards;

public class FlashcardResponse
{
    public int WordId { get; set; }
    public string EnglishWord { get; set; } = string.Empty;
    public IReadOnlyCollection<string> KyrgyzTranslations { get; set; } = [];
}
