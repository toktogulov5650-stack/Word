using Word.Application.DTOs.WordExplanations;

namespace Word.Application.Abstractions.Services;

public interface IWordExplanationService
{
    Task<WordExplanationDto?> GetByWordIdAsync(
        int wordId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<WordExplanationDto>> GetByWordIdsAsync(
        IReadOnlyCollection<int> wordIds,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<WordExplanationDto>> GetByCategoryIdAsync(
        int categoryId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<UnknownWordDto>> GetMarkedUnknownByTestSessionIdAsync(
        int testSessionId,
        CancellationToken cancellationToken = default);
}
