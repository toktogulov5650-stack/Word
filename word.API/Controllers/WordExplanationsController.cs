using Microsoft.AspNetCore.Mvc;
using Word.Application.Abstractions.Services;
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
    public async Task<ActionResult<IReadOnlyCollection<UnknownWordResponse>>>
        GetMarkedUnknownByTestSessionIdAsync(int testSessionId, CancellationToken cancellationToken = default)
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
        CancellationToken cancellationToken = default)
    {
        var result = await _wordExplanationService.GetByWordIdAsync(wordId, cancellationToken);

        if (result is null)
            return NotFound("Разбор слова не найден");

        var response = new WordExplanationResponse
        {
            WordId = result.WordId,
            EnglishWord = result.EnglishWord,
            WhatIs = result.WhatIs,
            Meaning = result.Meaning,
            Translations = result.Translations,
            Usage = result.Usage,
            Example1 = result.Example1,
            Example2 = result.Example2,
            Example3 = result.Example3,
            Hint = result.Hint
        };

        return Ok(response);
    }

    [HttpGet("categories/{categoryId:int}")]
    public async Task<ActionResult<IReadOnlyCollection<WordExplanationResponse>>> GetByCategoryIdAsync(
        int categoryId,
        CancellationToken cancellationToken = default)
    {
        var result = await _wordExplanationService.GetByCategoryIdAsync(categoryId, cancellationToken);

        var response = result
            .Select(x => new WordExplanationResponse
            {
                WordId = x.WordId,
                EnglishWord = x.EnglishWord,
                WhatIs = x.WhatIs,
                Meaning = x.Meaning,
                Translations = x.Translations,
                Usage = x.Usage,
                Example1 = x.Example1,
                Example2 = x.Example2,
                Example3 = x.Example3,
                Hint = x.Hint
            })
            .ToList();

        return Ok(response);
    }
}
