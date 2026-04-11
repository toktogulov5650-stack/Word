using Word.Domain.Enums;

namespace Word.Domain.Entities;

public class TestSession
{
    private readonly List<TestQuestion> _testQuestions = new();

    public int Id { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public TestSessionStatus Status { get; set; }
    public int CorrectAnswerCount { get; set; }
    public int TotalQuestionCount { get; set; }

    public IReadOnlyCollection<TestQuestion> TestQuestions => _testQuestions;

}
