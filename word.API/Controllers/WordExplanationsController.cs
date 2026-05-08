using Microsoft.AspNetCore.Mvc;
using Word.Application.Abstractions.Services;
using Word.Application.DTOs.WordExplanations;
using Word.API.Contracts.WordExplanations;

namespace Word.API.Controllers;

[ApiController]
[Route("api/word-explanations")]
public class WordExplanationsController : ControllerBase
{
    private readonly IWordExplanationService _wordExplanationService;

    public WordExplanationsController(IWordExplanationService wordExplanationService)
    {
        _wordExplanationService = wordExplanationService;
    }

    [HttpGet("tests/{testSessionId:int}/unknown-words")]
    public async Task<ActionResult<IReadOnlyCollection<UnknownWordResponse>>> GetMarkedUnknownByTestSessionIdAsync(
        int testSessionId,
        CancellationToken cancellationToken = default)
    {
        var result = await _wordExplanationService.GetMarkedUnknownByTestSessionIdAsync(
            testSessionId,
            cancellationToken);

        var response = result
            .Select(a => new UnknownWordResponse
            {
                WordId = a.WordId,
                EnglishWord = a.EnglishWord,
                PrimaryTranslation = a.PrimaryTranslation
            })
            .ToList();

        return Ok(response);
    }

    [HttpGet("words/{wordId:int}")]
    public async Task<ActionResult<WordExplanationResponse>> GetByWordIdAsync(
        int wordId,
        [FromQuery] string? lang = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _wordExplanationService.GetByWordIdAsync(wordId, lang, cancellationToken);

        if (result is null)
            return NotFound("Разбор слова не найден");

        return Ok(MapResponse(result));
    }

    [HttpGet("categories/{categoryId:int}")]
    public async Task<ActionResult<IReadOnlyCollection<WordExplanationResponse>>> GetByCategoryIdAsync(
        int categoryId,
        [FromQuery] string? lang = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _wordExplanationService.GetByCategoryIdAsync(categoryId, lang, cancellationToken);
        return Ok(result.Select(MapResponse).ToList());
    }

    private static WordExplanationResponse MapResponse(WordExplanationDto dto)
    {
        return new WordExplanationResponse
        {
            WordId = dto.WordId,
            EnglishWord = dto.EnglishWord,
            WhatIs = dto.WhatIs,
            Meaning = dto.Meaning,
            Translations = dto.Translations,
            Usage = dto.Usage,
            Hint = dto.Hint,
            Examples = dto.Examples
                .Select(x => new WordExampleResponse
                {
                    Order = x.Order,
                    Text = x.Text,
                    Translation = x.Translation
                })
                .ToList()
        };
    }
}
