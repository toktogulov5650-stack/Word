using Microsoft.AspNetCore.Mvc;
using Word.API.Contracts.Flashcards;
using Word.API.Services;
using Word.Application.Abstractions.Services;

namespace Word.API.Controllers;

[ApiController]
[Route("api/flashcards")]
public class FlashcardsController : ControllerBase
{
    private readonly IFlashcardService _flashcardService;
    private readonly IEffectiveLanguageResolver _languageResolver;

    public FlashcardsController(
        IFlashcardService flashcardService,
        IEffectiveLanguageResolver languageResolver)
    {
        _flashcardService = flashcardService;
        _languageResolver = languageResolver;
    }

    [HttpGet("random")]
    public async Task<ActionResult<FlashcardResponse>> GetRandomAsync(
        [FromQuery] string? lang = null,
        [FromQuery] int? excludeWordId = null,
        CancellationToken cancellationToken = default)
    {
        var effectiveLanguage = await _languageResolver.ResolveAsync(lang, cancellationToken);
        var flashcard = await _flashcardService.GetRandomAsync(effectiveLanguage, excludeWordId, cancellationToken);

        return Ok(new FlashcardResponse
        {
            WordId = flashcard.WordId,
            EnglishWord = flashcard.EnglishWord,
            Translations = flashcard.Translations
        });
    }
}
