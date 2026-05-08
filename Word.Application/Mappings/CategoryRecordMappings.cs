using Word.Application.DTOs.Records;
using Word.Application.Localization;
using Word.Domain.Entities;

namespace Word.Application.Mappings;

public static class CategoryRecordMappings
{
    public static CategoryRecordDto ToCategoryRecordDto(this CategoryRecord categoryRecord)
    {
        var translation = LocalizedContentResolver.ResolveTranslation(
            categoryRecord.Category.Translations,
            LocalizedContentResolver.DefaultLanguageCode,
            x => x.LanguageCode);

        return new CategoryRecordDto
        {
            CategoryId = categoryRecord.CategoryId,
            CategoryName = translation?.Name ?? string.Empty,
            BestScore = categoryRecord.BestScore
        };
    }
}
