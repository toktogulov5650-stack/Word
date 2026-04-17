using Microsoft.EntityFrameworkCore;
using Word.Domain.Entities;
using Word.Application.Abstractions.Persistence;
using Word.Infrastructure.Persistence;


namespace Word.Infrastructure.Repositories;

public class WordRepository : IWordRepository
{
    private readonly AppDbContext _appDbContext;

    public WordRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }


    public async Task<IReadOnlyCollection<WordEntity>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.WordEntities
            .Where(a => a.CategoryId == categoryId && a.IsActive)
            .ToListAsync(cancellationToken);
    }


    public async Task<IReadOnlyCollection<WordEntity>> GetByIdsAsync(
        IReadOnlyCollection<int> ids,
        CancellationToken cancellationToken = default)
    {
        if (ids.Count == 0)
            return [];


        return await _appDbContext.WordEntities
            .Where(a => ids.Contains(a.Id) && a.IsActive)
            .ToListAsync(cancellationToken);
    }
}

