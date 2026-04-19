namespace Word.API.Contracts.WordExplanations;

public class UnknownWordResponse
{
    public int WordId { get; set; }

    public string EnglishWord { get; set; } = string.Empty;

    public string PrimaryTranslation { get; set; } = string.Empty;
}
