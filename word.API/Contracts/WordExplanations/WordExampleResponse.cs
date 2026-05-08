namespace Word.API.Contracts.WordExplanations;

public class WordExampleResponse
{
    public int Order { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Translation { get; set; } = string.Empty;
}
