using Word.Domain.Entities;

namespace Word.Application.Abstractions.Persistence;

public interface IWordRepository
{
    Task<IReadOnlyCollection<WordEntity>> GetByCategoryIdAsync(int categoryId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<WordEntity>> GetByIdsAsync(IReadOnlyCollection<int> ids,
        CancellationToken cancellationToken = default);
}
