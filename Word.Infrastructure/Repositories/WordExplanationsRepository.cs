using Microsoft.EntityFrameworkCore;
using Word.Application.Abstractions.Persistence;
using Word.Domain.Entities;
using Word.Infrastructure.Persistence;


namespace Word.Infrastructure.Repositories;

public class WordExplanationsRepository : IWordExplanationRepository
{
    private readonly AppDbContext _appDbContext;

    public WordExplanationsRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }


    public async Task<WordExplanation?> GetByWordIdAsync(
        int wordId,
        CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Set<WordExplanation>()
            .Include(a => a.Word)
            .FirstOrDefaultAsync(a => a.WordId == wordId, cancellationToken);
    }


    public async Task<IReadOnlyCollection<WordExplanation>> GetByWordIdsAsync(
        IReadOnlyCollection<int> wordIds,
        CancellationToken cancellationToken = default)
    {
        if (wordIds.Count == 0)
            return [];


        return await _appDbContext.Set<WordExplanation>()
            .Include(a => a.Word)
            .Where(a => wordIds.Contains(a.WordId))
            .ToListAsync(cancellationToken);
    }



    public async Task<IReadOnlyCollection<WordExplanation>> GetByCategoryIdAsync(
        int categoryId,
        CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Set<WordExplanation>()
            .Include(x => x.Word)
            .Where(x => x.Word.CategoryId == categoryId && x.Word.IsActive)
            .ToListAsync(cancellationToken);
    }
}
