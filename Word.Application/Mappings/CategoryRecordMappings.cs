using Word.Domain.Entities;
using Word.Application.DTOs.Records;


namespace Word.Application.Mappings;

public static class CategoryRecordMappings
{
    public static CategoryRecordDto ToCategoryRecordDto(this CategoryRecord categoryRecord)
    {
        return new CategoryRecordDto
        {
            CategoryId = categoryRecord.CategoryId,
            CategoryName = categoryRecord.Category.Name,
            BestScore = categoryRecord.BestScore
        };
    }
}
