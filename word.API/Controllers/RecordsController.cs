using Microsoft.AspNetCore.Mvc;
using Word.API.Contracts.Records;
using Word.API.Services;
using Word.Application.Abstractions.Services;

namespace Word.API.Controllers;

[ApiController]
[Route("api/records")]
public class RecordsController : ControllerBase
{
    private readonly IRecordService _recordService;
    private readonly IEffectiveLanguageResolver _languageResolver;

    public RecordsController(
        IRecordService recordService,
        IEffectiveLanguageResolver languageResolver)
    {
        _recordService = recordService;
        _languageResolver = languageResolver;
    }

    [HttpGet("categories/{categoryId:int}")]
    public async Task<ActionResult<CategoryRecordResponse>> GetByCategoryIdAsync(
        int categoryId,
        [FromQuery] string? lang = null,
        CancellationToken cancellationToken = default)
    {
        var effectiveLanguage = await _languageResolver.ResolveAsync(lang, cancellationToken);
        var result = await _recordService.GetByCategoryIdAsync(categoryId, effectiveLanguage, cancellationToken);

        if (result is null)
            return NotFound("Category record was not found.");

        var response = new CategoryRecordResponse
        {
            CategoryId = result.CategoryId,
            CategoryName = result.CategoryName,
            BestScore = result.BestScore,
            BestTotalQuestionCount = result.BestTotalQuestionCount
        };

        return Ok(response);
    }
}
