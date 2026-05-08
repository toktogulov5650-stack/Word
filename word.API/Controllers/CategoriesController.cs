using Microsoft.AspNetCore.Mvc;
using Word.API.Contracts.Categories;
using Word.API.Services;
using Word.Application.Abstractions.Services;

namespace Word.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IEffectiveLanguageResolver _languageResolver;

    public CategoriesController(
        ICategoryService categoryService,
        IEffectiveLanguageResolver languageResolver)
    {
        _categoryService = categoryService;
        _languageResolver = languageResolver;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CategoryResponse>>> GetAllAsync(
        [FromQuery] string? lang = null,
        CancellationToken cancellationToken = default)
    {
        var effectiveLanguage = await _languageResolver.ResolveAsync(lang, cancellationToken);
        var result = await _categoryService.GetAllAsync(effectiveLanguage, cancellationToken);

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
