


namespace Word.API.Contracts.Tests;

public class FinishTestResponse
{
    public int CorrectAnswerCount { get; set; }
    public int TotalQuestionCount { get; set; }
    public int BestScore { get; set; }
    public int BestTotalQuestionCount { get; set; }
}
