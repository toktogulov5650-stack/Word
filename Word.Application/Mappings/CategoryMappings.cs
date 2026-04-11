using Word.Domain.Entities;
using Word.Application.DTOs.Categories;


namespace Word.Application.Mappings;

public static class CategoryMappings
{

    public static CategoryDto ToCategoryDto(this Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }
}
