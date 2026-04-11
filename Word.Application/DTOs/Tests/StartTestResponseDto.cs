


namespace Word.Application.DTOs.Tests;

public class StartTestResponseDto
{
    public int TestSessionId { get; set; }
    public CurrentQuestionDto CurrentQuestion { get; set; } = new();
}
