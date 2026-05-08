using Word.Application.DTOs.WordExplanations;

namespace Word.Application.Abstractions.Services;

public interface IWordExplanationService
{
    Task<WordExplanationDto?> GetByWordIdAsync(
        int wordId,
        string? languageCode = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<WordExplanationDto>> GetByWordIdsAsync(
        IReadOnlyCollection<int> wordIds,
        string? languageCode = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<WordExplanationDto>> GetByCategoryIdAsync(
        int categoryId,
        string? languageCode = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<UnknownWordDto>> GetMarkedUnknownByTestSessionIdAsync(
        int testSessionId,
        CancellationToken cancellationToken = default);
}
