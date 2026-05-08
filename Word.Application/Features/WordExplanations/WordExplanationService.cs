using Word.Application.Abstractions.Persistence;
using Word.Application.Abstractions.Services;
using Word.Application.DTOs.WordExplanations;
using Word.Application.Localization;
using Word.Domain.Entities;

namespace Word.Application.Features.WordExplanations;

public class WordExplanationService : IWordExplanationService
{
    private readonly IWordExplanationRepository _wordExplanationRepository;
    private readonly ITestQuestionRepository _testQuestionRepository;
    private readonly ITestSessionRepository _testSessionRepository;

    public WordExplanationService(
        IWordExplanationRepository wordExplanationRepository,
        ITestQuestionRepository testQuestionRepository,
        ITestSessionRepository testSessionRepository)
    {
        _wordExplanationRepository = wordExplanationRepository;
        _testQuestionRepository = testQuestionRepository;
        _testSessionRepository = testSessionRepository;
    }

    public async Task<WordExplanationDto?> GetByWordIdAsync(
        int wordId,
        string? languageCode = null,
        CancellationToken cancellationToken = default)
    {
        var wordExplanation = await _wordExplanationRepository.GetByWordIdAsync(wordId, cancellationToken);
        return wordExplanation is null ? null : MapWordExplanation(wordExplanation, languageCode);
    }

    public async Task<IReadOnlyCollection<WordExplanationDto>> GetByWordIdsAsync(
        IReadOnlyCollection<int> wordIds,
        string? languageCode = null,
        CancellationToken cancellationToken = default)
    {
        var wordExplanations = await _wordExplanationRepository.GetByWordIdsAsync(wordIds, cancellationToken);

        return wordExplanations
            .Select(wordExplanation => MapWordExplanation(wordExplanation, languageCode))
            .ToList();
    }

    public async Task<IReadOnlyCollection<WordExplanationDto>> GetByCategoryIdAsync(
        int categoryId,
        string? languageCode = null,
        CancellationToken cancellationToken = default)
    {
        var wordExplanations = await _wordExplanationRepository.GetByCategoryIdAsync(categoryId, cancellationToken);

        return wordExplanations
            .Select(wordExplanation => MapWordExplanation(wordExplanation, languageCode))
            .ToList();
    }

    public async Task<IReadOnlyCollection<UnknownWordDto>> GetMarkedUnknownByTestSessionIdAsync(
        int testSessionId,
        CancellationToken cancellationToken = default)
    {
        var testSession = await _testSessionRepository.GetByIdAsync(testSessionId, cancellationToken);

        if (testSession is null)
            throw new KeyNotFoundException("Test session was not found.");

        var testQuestions = await _testQuestionRepository
            .GetMarkedUnknownByTestSessionIdAsync(testSessionId, cancellationToken);

        return testQuestions
            .Select(testQuestion => new UnknownWordDto
            {
                WordId = testQuestion.WordId,
                EnglishWord = testQuestion.Word.EnglishWord,
                PrimaryTranslation = LocalizedContentResolver.ResolveTranslations(
                        testQuestion.Word.WordTranslations,
                        testSession.LanguageCode,
                        x => x.LanguageCode)
                    .Select(x => x.Text)
                    .FirstOrDefault() ?? string.Empty
            })
            .ToList();
    }

    private static WordExplanationDto MapWordExplanation(
        WordExplanation wordExplanation,
        string? languageCode)
    {
        var translation = LocalizedContentResolver.ResolveTranslation(
            wordExplanation.Translations,
            languageCode,
            x => x.LanguageCode);

        return new WordExplanationDto
        {
            WordId = wordExplanation.WordId,
            EnglishWord = wordExplanation.Word.EnglishWord,
            WhatIs = translation?.WhatIs ?? string.Empty,
            Meaning = translation?.Meaning ?? string.Empty,
            Translations = translation?.Translations ?? string.Empty,
            Usage = translation?.Usage ?? string.Empty,
            Hint = translation?.Hint ?? string.Empty,
            Examples = wordExplanation.Examples
                .OrderBy(x => x.SortOrder)
                .Select(example =>
                {
                    var exampleTranslation = LocalizedContentResolver.ResolveTranslation(
                        example.Translations,
                        languageCode,
                        x => x.LanguageCode);

                    return new WordExampleDto
                    {
                        Order = example.SortOrder,
                        Text = exampleTranslation?.Text ?? string.Empty,
                        Translation = exampleTranslation?.Translation ?? string.Empty
                    };
                })
                .ToList()
        };
    }
}
