using Microsoft.AspNetCore.Mvc;
using Word.Application.Abstractions.Services;
using Word.API.Contracts.Categories;

namespace Word.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CategoryResponse>>> GetAllAsync(
        [FromQuery] string? lang = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _categoryService.GetAllAsync(lang, cancellationToken);

        var response = result.Select(dto => new CategoryResponse
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl
        }).ToList();

        return Ok(response);
    }
}
