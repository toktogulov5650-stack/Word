namespace Word.Application.DTOs.Flashcards;

public class FlashcardDto
{
    public int WordId { get; set; }
    public string EnglishWord { get; set; } = string.Empty;
    public IReadOnlyCollection<string> KyrgyzTranslations { get; set; } = [];
}
