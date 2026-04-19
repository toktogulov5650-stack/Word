namespace Word.Application.DTOs.WordExplanations;

public class UnknownWordDto
{
    public int WordId { get; set; }

    public string EnglishWord { get; set; } = string.Empty;

    public string PrimaryTranslation { get; set; } = string.Empty;
}
