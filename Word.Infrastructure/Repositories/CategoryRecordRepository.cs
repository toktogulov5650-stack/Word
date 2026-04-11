using Microsoft.EntityFrameworkCore;
using Word.Application.Abstractions.Persistence;
using Word.Infrastructure.Persistence;
using Word.Domain.Entities;


namespace Word.Infrastructure.Repositories;

public class CategoryRecordRepository : ICategoryRecordRepository
{
    private readonly AppDbContext _appDbContext;

    public CategoryRecordRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }


    public async Task<CategoryRecord?> GetByCategoryIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.CategoryRecords
            .Include(a => a.Category)
            .FirstOrDefaultAsync(a => a.CategoryId == id, cancellationToken);
    }


    public async Task AddAsync(CategoryRecord categoryRecord, CancellationToken cancellationToken = default)
    {
        await _appDbContext.AddAsync(categoryRecord, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }


    public async Task UpdateAsync(CategoryRecord categoryRecord, CancellationToken cancellationToken = default)
    {
        _appDbContext.Update(categoryRecord);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
