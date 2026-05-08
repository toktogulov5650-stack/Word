using Word.Application.DTOs.Categories;
using Word.Application.Localization;
using Word.Domain.Entities;

namespace Word.Application.Mappings;

public static class CategoryMappings
{
    public static CategoryDto ToCategoryDto(this Category category, string? languageCode)
    {
        var translation = LocalizedContentResolver.ResolveTranslation(
            category.Translations,
            languageCode,
            x => x.LanguageCode);

        return new CategoryDto
        {
            Id = category.Id,
            Name = translation?.Name ?? string.Empty,
            Description = translation?.Description ?? string.Empty,
            ImageUrl = category.ImageUrl
        };
    }
}
