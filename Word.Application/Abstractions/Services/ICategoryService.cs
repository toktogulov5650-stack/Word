using Word.Application.DTOs.Categories;


namespace Word.Application.Abstractions.Services;

public interface ICategoryService
{
    Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
