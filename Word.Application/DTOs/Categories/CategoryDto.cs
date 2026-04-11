using System.ComponentModel.DataAnnotations;


namespace Word.Application.DTOs.Categories;

public class CategoryDto 
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;
}
