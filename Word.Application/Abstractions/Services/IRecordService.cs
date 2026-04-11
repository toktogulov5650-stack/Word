using Word.Application.DTOs.Records;


namespace Word.Application.Abstractions.Services;

public interface IRecordService
{
    Task<CategoryRecordDto?> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
}
