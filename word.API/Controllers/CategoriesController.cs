using Microsoft.AspNetCore.Mvc;
using Word.Application.Abstractions.Services;
using Word.Application.DTOs.Categories;
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
    public async Task<ActionResult<IReadOnlyCollection<CategoryResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _categoryService.GetAllAsync(cancellationToken);

        var response = result.Select(dto => new CategoryResponse
        {
            Id = dto.Id,
            Name = dto.Name
        }).ToList();


        return Ok(response);
    }
}
 