using Word.Application.DTOs.Flashcards;

namespace Word.Application.Abstractions.Services;

public interface IFlashcardService
{
    Task<FlashcardDto> GetRandomAsync(
        string? languageCode = null,
        int? excludeWordId = null,
        CancellationToken cancellationToken = default);
}
