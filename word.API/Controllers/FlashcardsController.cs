using Microsoft.AspNetCore.Mvc;
using Word.API.Contracts.Flashcards;
using Word.Application.Abstractions.Services;

namespace Word.API.Controllers;

[ApiController]
[Route("api/flashcards")]
public class FlashcardsController : ControllerBase
{
    private readonly IFlashcardService _flashcardService;

    public FlashcardsController(IFlashcardService flashcardService)
    {
        _flashcardService = flashcardService;
    }

    [HttpGet("random")]
    public async Task<ActionResult<FlashcardResponse>> GetRandomAsync(
        [FromQuery] int? excludeWordId = null,
        CancellationToken cancellationToken = default)
    {
        var flashcard = await _flashcardService.GetRandomAsync(excludeWordId, cancellationToken);

        return Ok(new FlashcardResponse
        {
            WordId = flashcard.WordId,
            EnglishWord = flashcard.EnglishWord,
            KyrgyzTranslations = flashcard.KyrgyzTranslations
        });
    }
}
