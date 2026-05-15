using Word.Application.Abstractions.Persistence;
using Word.Application.Abstractions.Services;
using Word.Application.DTOs.Tests;
using Word.Application.Localization;
using Word.Domain.Entities;
using Word.Domain.Enums;

namespace Word.Application.Features.Tests;

public class TestService : ITestService
{
    private const int MinimumAnswerOptions = 4;

    private readonly ITestSessionRepository _testSessionRepository;
    private readonly ITestQuestionRepository _testQuestionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IWordRepository _wordRepository;
    private readonly ICategoryRecordRepository _categoryRecordRepository;

    public TestService(
        ITestSessionRepository testSessionRepository,
        ITestQuestionRepository testQuestionRepository,
        ICategoryRepository categoryRepository,
        IWordRepository wordRepository,
        ICategoryRecordRepository categoryRecordRepository)
    {
        _testSessionRepository = testSessionRepository;
        _testQuestionRepository = testQuestionRepository;
        _categoryRepository = categoryRepository;
        _wordRepository = wordRepository;
        _categoryRecordRepository = categoryRecordRepository;
    }

    public async Task<StartTestResponseDto> StartAsync(
        StartTestRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (request.CategoryId <= 0)
            throw new ArgumentException("Category ID must be greater than zero.", nameof(request.CategoryId));

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
            throw new KeyNotFoundException("Category was not found.");

        var normalizedLanguageCode = LocalizedContentResolver.NormalizeRequestedLanguage(request.LanguageCode);
        var words = (await _wordRepository.GetByCategoryIdAsync(request.CategoryId, cancellationToken))
            .Where(x => ResolveWordTranslations(x, normalizedLanguageCode).Count > 0)
            .ToList();

        if (words.Count == 0)
            throw new InvalidOperationException("This category does not contain any active words with translations.");

        if (words.Count < MinimumAnswerOptions)
            throw new InvalidOperationException($"At least {MinimumAnswerOptions} words are required to start a test.");

        var testSession = new TestSession
        {
            CategoryId = request.CategoryId,
            LanguageCode = normalizedLanguageCode,
            Status = TestSessionStatus.InProgress,
            CorrectAnswerCount = 0,
            TotalQuestionCount = words.Count
        };

        await _testSessionRepository.AddAsync(testSession, cancellationToken);

        var shuffledWords = words
            .OrderBy(_ => Random.Shared.Next())
            .ToList();

        var testQuestions = shuffledWords
            .Select((word, index) => new TestQuestion(
                testSession.Id,
                word.Id,
                index + 1,
                false,
                false,
                false))
            .ToList();

        await _testQuestionRepository.AddRangeAsync(testQuestions, cancellationToken);

        return new StartTestResponseDto
        {
            TestSessionId = testSession.Id,
            TotalQuestionCount = testSession.TotalQuestionCount,
            CurrentQuestion = BuildCurrentQuestion(shuffledWords[0], words, normalizedLanguageCode)
        };
    }

    public async Task<SubmitAnswerResponseDto> SubmitAnswerAsync(
        SubmitAnswerRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ValidateSubmitAnswerRequest(request);

        var testSession = await _testSessionRepository.GetByIdAsync(request.TestSessionId, cancellationToken);

        if (testSession is null)
            throw new KeyNotFoundException("Test session was not found.");

        if (testSession.Status == TestSessionStatus.Completed)
            throw new InvalidOperationException("This test session has already been completed.");

        var currentQuestion = await _testQuestionRepository
            .GetByTestSessionIdAndWordIdAsync(request.TestSessionId, request.WordId, cancellationToken);

        if (currentQuestion is null)
            throw new KeyNotFoundException("Question was not found in this test session.");

        if (currentQuestion.IsAnswered)
            throw new InvalidOperationException("This question has already been answered.");

        var isCorrect = request.IsMarkedUnknown
            ? MarkQuestionAsUnknown(currentQuestion)
            : CheckAnswer(currentQuestion, request.SelectedAnswer, testSession.LanguageCode);

        await _testQuestionRepository.UpdateAsync(currentQuestion, cancellationToken);

        if (isCorrect)
            testSession.CorrectAnswerCount++;

        var nextQuestion = await _testQuestionRepository.GetNextUnansweredAsync(request.TestSessionId, cancellationToken);

        if (nextQuestion is null)
        {
            testSession.Status = TestSessionStatus.Completed;
            await _testSessionRepository.UpdateAsync(testSession, cancellationToken);

            return new SubmitAnswerResponseDto
            {
                IsCorrect = isCorrect,
                CorrectAnswerCount = testSession.CorrectAnswerCount,
                TotalQuestionCount = testSession.TotalQuestionCount,
                IsFinished = true,
                CurrentQuestion = null
            };
        }

        await _testSessionRepository.UpdateAsync(testSession, cancellationToken);

        var words = (await _wordRepository.GetByCategoryIdAsync(testSession.CategoryId, cancellationToken))
            .Where(x => ResolveWordTranslations(x, testSession.LanguageCode).Count > 0)
            .ToList();

        var nextWord = words.FirstOrDefault(x => x.Id == nextQuestion.WordId)
            ?? throw new KeyNotFoundException("The next word for this test session was not found.");

        return new SubmitAnswerResponseDto
        {
            IsCorrect = isCorrect,
            CorrectAnswerCount = testSession.CorrectAnswerCount,
            TotalQuestionCount = testSession.TotalQuestionCount,
            IsFinished = false,
            CurrentQuestion = BuildCurrentQuestion(nextWord, words, testSession.LanguageCode)
        };
    }

    public async Task<FinishTestResponseDto> FinishAsync(
        int testSessionId,
        CancellationToken cancellationToken = default)
    {
        if (testSessionId <= 0)
            throw new ArgumentException("Test session ID must be greater than zero.", nameof(testSessionId));

        var testSession = await _testSessionRepository.GetByIdAsync(testSessionId, cancellationToken);

        if (testSession is null)
            throw new KeyNotFoundException("Test session was not found.");

        if (testSession.Status != TestSessionStatus.Completed)
        {
            testSession.Status = TestSessionStatus.Completed;
            await _testSessionRepository.UpdateAsync(testSession, cancellationToken);
        }

        var record = await _categoryRecordRepository.GetByCategoryIdAsync(testSession.CategoryId, cancellationToken);

        if (record is null)
        {
            record = new CategoryRecord(
                testSession.CategoryId,
                testSession.CorrectAnswerCount,
                testSession.TotalQuestionCount);

            await _categoryRecordRepository.AddAsync(record, cancellationToken);
        }
        else if (testSession.CorrectAnswerCount > record.BestScore)
        {
            record.UpdateBestScore(testSession.CorrectAnswerCount, testSession.TotalQuestionCount);
            await _categoryRecordRepository.UpdateAsync(record, cancellationToken);
        }
        else if (record.BestTotalQuestionCount == 0 && testSession.CorrectAnswerCount == record.BestScore)
        {
            record.UpdateBestScore(record.BestScore, testSession.TotalQuestionCount);
            await _categoryRecordRepository.UpdateAsync(record, cancellationToken);
        }

        return new FinishTestResponseDto
        {
            CorrectAnswerCount = testSession.CorrectAnswerCount,
            TotalQuestionCount = testSession.TotalQuestionCount,
            BestScore = record.BestScore,
            BestTotalQuestionCount = record.BestTotalQuestionCount
        };
    }

    private static CurrentQuestionDto BuildCurrentQuestion(
        WordEntity word,
        IReadOnlyCollection<WordEntity> words,
        string languageCode)
    {
        return new CurrentQuestionDto
        {
            WordId = word.Id,
            EnglishWord = word.EnglishWord,
            AnswerOptions = GenerateAnswerOptions(word, words, languageCode)
        };
    }

    private static void ValidateSubmitAnswerRequest(SubmitAnswerRequestDto request)
    {
        if (request.TestSessionId <= 0)
            throw new ArgumentException("Test session ID must be greater than zero.", nameof(request.TestSessionId));

        if (request.WordId <= 0)
            throw new ArgumentException("Word ID must be greater than zero.", nameof(request.WordId));

        if (!request.IsMarkedUnknown && string.IsNullOrWhiteSpace(request.SelectedAnswer))
            throw new ArgumentException("Selected answer is required when the word is not marked as unknown.", nameof(request.SelectedAnswer));
    }

    private static bool MarkQuestionAsUnknown(TestQuestion currentQuestion)
    {
        currentQuestion.MarkUnknown();
        currentQuestion.MarkAnswered(false);
        return false;
    }

    private static bool CheckAnswer(TestQuestion currentQuestion, string selectedAnswer, string languageCode)
    {
        var normalizedAnswer = selectedAnswer.Trim();

        var isCorrect = ResolveWordTranslations(currentQuestion.Word, languageCode)
            .Any(x => string.Equals(x, normalizedAnswer, StringComparison.OrdinalIgnoreCase));

        currentQuestion.MarkAnswered(isCorrect);
        return isCorrect;
    }

    private static IReadOnlyCollection<string> GenerateAnswerOptions(
        WordEntity currentWord,
        IReadOnlyCollection<WordEntity> words,
        string languageCode)
    {
        var currentTranslations = ResolveWordTranslations(currentWord, languageCode)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var correctAnswer = currentTranslations.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(correctAnswer))
            throw new InvalidOperationException("The current word does not have any translations.");

        var wrongAnswersPool = words
            .Where(x => x.Id != currentWord.Id)
            .SelectMany(x => ResolveWordTranslations(x, languageCode))
            .Where(x => currentTranslations.All(y => !string.Equals(y, x, StringComparison.OrdinalIgnoreCase)))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(_ => Guid.NewGuid())
            .ToList();

        if (wrongAnswersPool.Count < MinimumAnswerOptions - 1)
            throw new InvalidOperationException("Not enough unique answer options were found for this word.");

        return wrongAnswersPool
            .Take(MinimumAnswerOptions - 1)
            .Append(correctAnswer)
            .OrderBy(_ => Guid.NewGuid())
            .ToList();
    }

    private static IReadOnlyCollection<string> ResolveWordTranslations(WordEntity word, string? languageCode)
    {
        return LocalizedContentResolver.ResolveTranslations(
                word.WordTranslations,
                languageCode,
                x => x.LanguageCode)
            .Select(x => x.Text)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}
