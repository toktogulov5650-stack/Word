using Word.Domain.Entities;

namespace Word.Application.Abstractions.Persistence;

public interface ITestQuestionRepository
{
    Task AddRangeAsync(IReadOnlyCollection<TestQuestion> testQuestions, CancellationToken cancellationToken = default);
    Task<TestQuestion?> GetByTestSessionIdAndWordIdAsync(int testSessionId, int wordId, CancellationToken cancellationToken = default);
    Task<TestQuestion?> GetNextUnansweredAsync(int testSessionId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TestQuestion>> GetMarkedUnknownByTestSessionIdAsync(
        int testSessionId,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(TestQuestion testQuestion, CancellationToken cancellationToken = default);
}

