using Word.Application.Abstractions.Persistence;
using Word.Application.Abstractions.Services;
using Word.Application.DTOs.Flashcards;
using Word.Application.Localization;

namespace Word.Application.Features.Flashcards;

public class FlashcardService : IFlashcardService
{
    private readonly IWordRepository _wordRepository;

    public FlashcardService(IWordRepository wordRepository)
    {
        _wordRepository = wordRepository;
    }

    public async Task<FlashcardDto> GetRandomAsync(
        string? languageCode = null,
        int? excludeWordId = null,
        CancellationToken cancellationToken = default)
    {
        var word = await _wordRepository.GetRandomAsync(excludeWordId, cancellationToken);

        if (word is null)
            throw new InvalidOperationException("Flashcards are not available because there are no active words.");

        var translations = LocalizedContentResolver.ResolveTranslations(
                word.WordTranslations,
                languageCode,
                x => x.LanguageCode)
            .Select(x => x.Text)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (translations.Count == 0)
            throw new InvalidOperationException("The selected word does not have any translations.");

        return new FlashcardDto
        {
            WordId = word.Id,
            EnglishWord = word.EnglishWord,
            Translations = translations
        };
    }
}
