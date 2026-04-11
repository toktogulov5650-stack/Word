using Word.Application.Abstractions.Services;
using Word.Application.Mappings;
using Word.Application.Abstractions.Persistence;
using Word.Application.DTOs.Records;


namespace Word.Application.Features.Records;

public class RecordService : IRecordService
{
    private readonly ICategoryRecordRepository _categoryRecordRepository;

    public RecordService(ICategoryRecordRepository categoryRecordRepository)
    {
        _categoryRecordRepository = categoryRecordRepository;
    }


    public async Task<CategoryRecordDto?> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        var record = await _categoryRecordRepository.GetByCategoryIdAsync(categoryId, cancellationToken);

        if (record is null)
            return null;

        return record?.ToCategoryRecordDto();
    }
}
