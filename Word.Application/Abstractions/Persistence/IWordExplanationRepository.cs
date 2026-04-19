using Word.Domain.Entities;


namespace Word.Application.Abstractions.Persistence;

public interface IWordExplanationRepository
{
    Task<WordExplanation?> GetByWordIdAsync(
        int wordId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<WordExplanation>> GetByWordIdsAsync(
        IReadOnlyCollection<int> wordIds,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<WordExplanation>> GetByCategoryIdAsync(
        int categoryId,
        CancellationToken cancellationToken = default);
}
