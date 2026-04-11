
namespace Word.API.Contracts.Tests;

public class SubmitAnswerResponse
{
    public bool IsCorrect { get; set; }
    public int CorrectAnswerCount { get; set; }
    public bool IsFinished { get; set; }
    public CurrentQuestionResponse? CurrentQuestion { get; set; } = null;
}
