using Microsoft.EntityFrameworkCore;
using Word.Application.Abstractions.Persistence;
using Word.Domain.Entities;
using Word.Infrastructure.Persistence;

namespace Word.Infrastructure.Repositories;

public class TestQuestionRepository : ITestQuestionRepository
{
    private readonly AppDbContext _appDbContext;

    public TestQuestionRepository(AppDbContext appDbContext) 
    {
        _appDbContext = appDbContext;
    }

    public async Task AddRangeAsync(IReadOnlyCollection<TestQuestion> testQuestions, CancellationToken cancellationToken = default)
    {
        await _appDbContext.TestQuestions.AddRangeAsync(testQuestions, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TestQuestion?> GetByTestSessionIdAndWordIdAsync(int testSessionId, int wordId, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.TestQuestions
            .Include(a => a.Word)
            .FirstOrDefaultAsync(
                a => a.TestSessionId == testSessionId && a.WordId == wordId,
                cancellationToken);
    }

    public async Task<TestQuestion?> GetNextUnansweredAsync(int testSessionId, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.TestQuestions
            .Include(a => a.Word)
            .Where(a => a.TestSessionId == testSessionId && !a.IsAnswered)
            .OrderBy(a => a.QuestionOrder)
            .FirstOrDefaultAsync(cancellationToken);
    }


    public async Task<IReadOnlyCollection<TestQuestion>> GetMarkedUnknownByTestSessionIdAsync(
        int testSessionId,
        CancellationToken cancellationToken = default)
    {
        return await _appDbContext.TestQuestions
            .Include(a => a.Word)
            .Where(a => a.TestSessionId == testSessionId && a.IsMarkedUnknown)
            .OrderBy(a => a.QuestionOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(TestQuestion testQuestion, CancellationToken cancellationToken = default)
    {
        _appDbContext.TestQuestions.Update(testQuestion);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
