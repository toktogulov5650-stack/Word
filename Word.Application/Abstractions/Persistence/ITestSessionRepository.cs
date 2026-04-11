using Word.Domain.Entities;


namespace Word.Application.Abstractions.Persistence;

public interface ITestSessionRepository
{
    Task<TestSession?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(TestSession testSession, CancellationToken cancellationToken = default);
    Task UpdateAsync(TestSession testSession, CancellationToken cancellationToken = default);
}
