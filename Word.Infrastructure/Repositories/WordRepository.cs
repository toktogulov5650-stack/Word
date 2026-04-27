using Microsoft.EntityFrameworkCore;
using Word.Application.Abstractions.Persistence;
using Word.Domain.Entities;
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
            .Include(a => a.WordTranslations)
            .Where(a => a.CategoryId == categoryId && a.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<WordEntity?> GetRandomAsync(int? excludeWordId = null, CancellationToken cancellationToken = default)
    {
        var query = _appDbContext.WordEntities
            .Include(a => a.WordTranslations)
            .Where(a => a.IsActive && a.WordTranslations.Any());

        if (excludeWordId.HasValue)
        {
            var alternateExists = await query.AnyAsync(a => a.Id != excludeWordId.Value, cancellationToken);

            if (alternateExists)
            {
                query = query.Where(a => a.Id != excludeWordId.Value);
            }
        }

        var wordIds = await query
            .Select(a => a.Id)
            .ToListAsync(cancellationToken);

        if (wordIds.Count == 0)
        {
            return null;
        }

        var randomWordId = wordIds[Random.Shared.Next(wordIds.Count)];

        return await _appDbContext.WordEntities
            .Include(a => a.WordTranslations)
            .FirstOrDefaultAsync(a => a.Id == randomWordId, cancellationToken);
    }
}
