using Microsoft.EntityFrameworkCore;
using Word.Domain.Entities;
using Word.Infrastructure.Persistence;
using Word.Application.Abstractions.Persistence;


namespace Word.Infrastructure.Repositories;

public class TestSessionRepository : ITestSessionRepository
{
    private readonly AppDbContext _appDbContext;

    public TestSessionRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }


    public async Task<TestSession?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.TestSessions
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }


    public async Task AddAsync(TestSession testSession, CancellationToken cancellationToken = default)
    {
        await _appDbContext.AddAsync(testSession, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }


    public async Task UpdateAsync(TestSession testSession, CancellationToken cancellationToken = default)
    {
        _appDbContext.Update(testSession);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
