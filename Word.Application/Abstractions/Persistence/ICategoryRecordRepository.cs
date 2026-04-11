using Word.Domain.Entities;


namespace Word.Application.Abstractions.Persistence;

public interface ICategoryRecordRepository
{
    Task<CategoryRecord?> GetByCategoryIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(CategoryRecord categoryRecord, CancellationToken cancellationToken = default);
    Task UpdateAsync(CategoryRecord categoryRecord, CancellationToken cancellationToken = default);
}
