
namespace Word.Application.DTOs.Tests;

public class CurrentQuestionDto
{
    public int WordId { get; set; }
    public string EnglishWord { get; set; } = string.Empty;
    public IReadOnlyCollection<string> AnswerOptions { get; set; } = [];
}
