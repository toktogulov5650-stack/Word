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
}
