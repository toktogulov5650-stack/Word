using Microsoft.AspNetCore.Mvc;
using Word.Application.Abstractions.Services;
using Word.API.Contracts.Records;


namespace Word.API.Controllers;

[ApiController]
[Route("api/records")]
public class RecordsController : ControllerBase
{
    private readonly IRecordService _recordService;

    public RecordsController(IRecordService recordService)
    {
        _recordService = recordService;
    }

    [HttpGet("categories/{categoryId:int}")]
    public async Task<ActionResult<CategoryRecordResponse>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        var result = await _recordService.GetByCategoryIdAsync(categoryId, cancellationToken);

        if (result is null)
            return NotFound("Рекорд этого категории не найдено");

        var response = new CategoryRecordResponse
        {
            CategoryId = result.CategoryId,
            BestScore = result.BestScore
        };

        return Ok(response);
    }
}
