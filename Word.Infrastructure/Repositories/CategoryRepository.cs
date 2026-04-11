using Microsoft.EntityFrameworkCore;
using Word.Application.Abstractions.Persistence;
using Word.Infrastructure.Persistence;
using Word.Domain.Entities;


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
            .Where(a => a.IsActive)
            .ToListAsync(cancellationToken);
    }


    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Categories
            .Where(a => a.IsActive)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }
}
