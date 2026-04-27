using Word.Application.Abstractions.Persistence;
using Word.Application.Abstractions.Services;
using Word.Application.DTOs.WordExplanations;

namespace Word.Application.Features.WordExplanations;

public class WordExplanationService : IWordExplanationService
{
    private readonly IWordExplanationRepository _wordExplanationRepository;
    private readonly ITestQuestionRepository _testQuestionRepository;

    public WordExplanationService(
        IWordExplanationRepository wordExplanationRepository,
        ITestQuestionRepository testQuestionRepository)
    {
        _wordExplanationRepository = wordExplanationRepository;
        _testQuestionRepository = testQuestionRepository;
    }

    public async Task<WordExplanationDto?> GetByWordIdAsync(int wordId, CancellationToken cancellationToken = default)
    {
        var wordExplanation = await _wordExplanationRepository.GetByWordIdAsync(wordId, cancellationToken);

        if (wordExplanation is null)
            return null;

        return new WordExplanationDto
        {
            WordId = wordExplanation.WordId,
            EnglishWord = wordExplanation.Word.EnglishWord,
            WhatIs = wordExplanation.WhatIs,
            Meaning = wordExplanation.Meaning,
            Translations = wordExplanation.Translations,
            Usage = wordExplanation.Usage,
            Example1 = wordExplanation.Example1,
            Example2 = wordExplanation.Example2,
            Example3 = wordExplanation.Example3,
            Hint = wordExplanation.Hint
        };
    }

    public async Task<IReadOnlyCollection<WordExplanationDto>> GetByWordIdsAsync(
        IReadOnlyCollection<int> wordIds,
        CancellationToken cancellationToken = default)
    {
        var wordExplanations = await _wordExplanationRepository.GetByWordIdsAsync(wordIds, cancellationToken);

        return wordExplanations
            .Select(wordExplanations => new WordExplanationDto
            {
                WordId = wordExplanations.WordId,
                EnglishWord = wordExplanations.Word.EnglishWord,
                WhatIs = wordExplanations.WhatIs,
                Meaning = wordExplanations.Meaning,
                Translations = wordExplanations.Translations,
                Usage = wordExplanations.Usage,
                Example1 = wordExplanations.Example1,
                Example2 = wordExplanations.Example2,
                Example3 = wordExplanations.Example3,
                Hint = wordExplanations.Hint
            })
            .ToList();
    }

    public async Task<IReadOnlyCollection<WordExplanationDto>> GetByCategoryIdAsync(
       int categoryId,
       CancellationToken cancellationToken = default)
    {
        var wordExplanations = await _wordExplanationRepository.GetByCategoryIdAsync(categoryId, cancellationToken);

        return wordExplanations
            .Select(wordExplanation => new WordExplanationDto
            {
                WordId = wordExplanation.WordId,
                EnglishWord = wordExplanation.Word.EnglishWord,
                WhatIs = wordExplanation.WhatIs,
                Meaning = wordExplanation.Meaning,
                Translations = wordExplanation.Translations,
                Usage = wordExplanation.Usage,
                Example1 = wordExplanation.Example1,
                Example2 = wordExplanation.Example2,
                Example3 = wordExplanation.Example3,
                Hint = wordExplanation.Hint
            })
            .ToList();
    }

    public async Task<IReadOnlyCollection<UnknownWordDto>> GetMarkedUnknownByTestSessionIdAsync(
        int testSessionId,
        CancellationToken cancellationToken = default)
    {
        var testQuestion = await _testQuestionRepository.
            GetMarkedUnknownByTestSessionIdAsync(testSessionId, cancellationToken);

        return testQuestion
            .Select(testQuestion => new UnknownWordDto
            {
                WordId = testQuestion.WordId,
                EnglishWord = testQuestion.Word.EnglishWord,
                PrimaryTranslation = testQuestion.Word.WordTranslations
                    .Select(x => x.KyrgyzWord)
                    .FirstOrDefault() ?? string.Empty
            })
            .ToList();
    }
}
