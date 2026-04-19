namespace Word.Domain.Entities;

public class TestQuestion
{
    private TestQuestion()
    {
    }

    public TestQuestion(
        int testSessionId,
        int wordId,
        int questionOrder,
        bool isAnswered,
        bool isCorrect,
        bool isMarkedUnknown)

    {
        TestSessionId = testSessionId;
        WordId = wordId;
        QuestionOrder = questionOrder;
        IsAnswered = isAnswered;
        IsCorrect = isCorrect;
        IsMarkedUnknown = isMarkedUnknown;

    }

    public int Id { get; private set; }
    public int TestSessionId { get; private set; }
    public TestSession TestSession { get; private set; } = null!;
    public int WordId { get; private set; }
    public WordEntity Word { get; private set; } = null!;
    public int QuestionOrder { get; private set; }
    public bool IsAnswered { get; private set; }
    public bool IsCorrect { get; private set; }
    public bool IsMarkedUnknown { get; private set; }

    public void MarkAnswered(bool isCorrect)
    {
        IsAnswered = true;
        IsCorrect = isCorrect;
    }

    public void MarkUnknown()
    {
        IsMarkedUnknown = true;
    }
}
