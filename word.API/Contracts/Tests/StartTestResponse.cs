
namespace Word.API.Contracts.Tests;

public class StartTestResponse
{
    public int TestSessionId { get; set; }

    public CurrentQuestionResponse CurrentQuestion { get; set; } = new();
}
