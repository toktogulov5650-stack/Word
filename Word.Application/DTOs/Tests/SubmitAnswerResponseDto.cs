

namespace Word.Application.DTOs.Tests;

public class SubmitAnswerResponseDto
{
    public bool IsCorrect { get; set; }
    public int CorrectAnswerCount { get; set; }
    public bool IsFinished { get; set; }
    public CurrentQuestionDto? CurrentQuestion { get; set; }
}
