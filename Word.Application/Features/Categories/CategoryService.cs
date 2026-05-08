using Word.Application.Abstractions.Persistence;
using Word.Application.Abstractions.Services;
using Word.Application.DTOs.Categories;
using Word.Application.Localization;
using Word.Application.Mappings;

namespace Word.Application.Features.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(
        string? languageCode = null,
        CancellationToken cancellationToken = default)
    {
        var categoryList = await _categoryRepository.GetAllAsync(cancellationToken);
        var normalizedLanguageCode = LocalizedContentResolver.NormalizeRequestedLanguage(languageCode);

        return categoryList
            .Select(category => category.ToCategoryDto(normalizedLanguageCode))
            .ToList();
    }
}
