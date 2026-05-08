using Microsoft.EntityFrameworkCore;
using Word.Application.Abstractions.Persistence;
using Word.Domain.Entities;
using Word.Infrastructure.Persistence;

namespace Word.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _appDbContext;

    public CategoryRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Categories
            .AsNoTracking()
            .Include(x => x.Translations)
            .Where(x => x.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Categories
            .AsNoTracking()
            .Include(x => x.Translations)
            .Where(x => x.IsActive)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
