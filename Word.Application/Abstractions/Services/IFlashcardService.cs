using Word.Application.DTOs.Flashcards;

namespace Word.Application.Abstractions.Services;

public interface IFlashcardService
{
    Task<FlashcardDto> GetRandomAsync(int? excludeWordId = null, CancellationToken cancellationToken = default);
}
