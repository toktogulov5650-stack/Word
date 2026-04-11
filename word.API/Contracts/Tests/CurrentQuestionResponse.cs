using System.ComponentModel.DataAnnotations;


namespace Word.API.Contracts.Tests;

public class CurrentQuestionResponse
{
    public int WordId { get; set; }

    public string EnglishWord { get; set; } = string.Empty;

    public IReadOnlyCollection<string> AnswerOptions { get; set; } = Array.Empty<string>();
}
