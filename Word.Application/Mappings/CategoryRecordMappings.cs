using Word.Application.DTOs.Records;
using Word.Application.Localization;
using Word.Domain.Entities;

namespace Word.Application.Mappings;

public static class CategoryRecordMappings
{
    public static CategoryRecordDto ToCategoryRecordDto(this CategoryRecord categoryRecord, string? languageCode)
    {
        var translation = LocalizedContentResolver.ResolveTranslation(
            categoryRecord.Category.Translations,
            languageCode,
            x => x.LanguageCode);

        return new CategoryRecordDto
        {
            CategoryId = categoryRecord.CategoryId,
            CategoryName = translation?.Name ?? string.Empty,
            BestScore = categoryRecord.BestScore,
            BestTotalQuestionCount = categoryRecord.BestTotalQuestionCount
        };
    }
}
