using Word.Application.Abstractions.Persistence;
using Word.Application.Abstractions.Services;
using Word.Application.DTOs.Tests;
using Word.Domain.Entities;
using Word.Domain.Enums;

namespace Word.Application.Features.Tests;

public class TestService : ITestService
{
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
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
            throw new Exception("Выбранная категория не существует");

        var words = (await _wordRepository.GetByCategoryIdAsync(request.CategoryId, cancellationToken)).ToList();

        if (!words.Any())
            throw new Exception("В этой категории нет слов");

        if (words.Count < 4)
            throw new Exception("Для теста нужно минимум 4 слова в категории");

        var testSession = new TestSession
        {
            CategoryId = request.CategoryId,
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
                false))
            .ToList();

        await _testQuestionRepository.AddRangeAsync(testQuestions, cancellationToken);

        var firstQuestion = testQuestions.OrderBy(a => a.QuestionOrder).First();
        var currentWord = shuffledWords.First(a => a.Id == firstQuestion.WordId);

        return new StartTestResponseDto
        {
            TestSessionId = testSession.Id,
            CurrentQuestion = new CurrentQuestionDto
            {
                WordId = currentWord.Id,
                EnglishWord = currentWord.EnglishWord,
                AnswerOptions = GenerateAnswerOptions(currentWord, words)
            }
        };
    }

    public async Task<SubmitAnswerResponseDto> SubmitAnswerAsync(
        SubmitAnswerRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var testSession = await _testSessionRepository.GetByIdAsync(request.TestSessionId, cancellationToken);

        if (testSession is null)
            throw new Exception("Тестовая сессия не найдена");

        if (testSession.Status == TestSessionStatus.Completed)
            throw new Exception("Тест уже завершен");

        var currentQuestion = await _testQuestionRepository
            .GetByTestSessionIdAndWordIdAsync(request.TestSessionId, request.WordId, cancellationToken);

        if (currentQuestion is null)
            throw new Exception("Вопрос не найден");

        if (currentQuestion.IsAnswered)
            throw new Exception("На этот вопрос уже ответили");

        var isCorrect = string.Equals(
            currentQuestion.Word.KyrgyzWord,
            request.SelectedAnswer,
            StringComparison.OrdinalIgnoreCase);

        currentQuestion.MarkAnswered(isCorrect);
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
                IsFinished = true,
                CurrentQuestion = null
            };
        }

        await _testSessionRepository.UpdateAsync(testSession, cancellationToken);

        var words = (await _wordRepository.GetByCategoryIdAsync(testSession.CategoryId, cancellationToken)).ToList();
        var nextWord = words.First(a => a.Id == nextQuestion.WordId);

        return new SubmitAnswerResponseDto
        {
            IsCorrect = isCorrect,
            CorrectAnswerCount = testSession.CorrectAnswerCount,
            IsFinished = false,
            CurrentQuestion = new CurrentQuestionDto
            {
                WordId = nextWord.Id,
                EnglishWord = nextWord.EnglishWord,
                AnswerOptions = GenerateAnswerOptions(nextWord, words)
            }
        };
    }

    public async Task<FinishTestResponseDto> FinishAsync(
        int testSessionId,
        CancellationToken cancellationToken = default)
    {
        var testSession = await _testSessionRepository.GetByIdAsync(testSessionId, cancellationToken);

        if (testSession is null)
            throw new Exception("Тестовая сессия не найдена");

        if (testSession.Status != TestSessionStatus.Completed)
        {
            testSession.Status = TestSessionStatus.Completed;
            await _testSessionRepository.UpdateAsync(testSession, cancellationToken);
        }

        var record = await _categoryRecordRepository.GetByCategoryIdAsync(testSession.CategoryId, cancellationToken);

        if (record is null)
        {
            record = new CategoryRecord(testSession.CategoryId, testSession.CorrectAnswerCount);
            await _categoryRecordRepository.AddAsync(record, cancellationToken);
        }
        else if (testSession.CorrectAnswerCount > record.BestScore)
        {
            record.UpdateBestScore(testSession.CorrectAnswerCount);
            await _categoryRecordRepository.UpdateAsync(record, cancellationToken);
        }

        return new FinishTestResponseDto
        {
            CorrectAnswerCount = testSession.CorrectAnswerCount,
            BestScore = record.BestScore
        };
    }

    private static IReadOnlyCollection<string> GenerateAnswerOptions(
        WordEntity currentWord,
        IReadOnlyCollection<WordEntity> words)
    {
        var wrongAnswersPool = words
            .Where(a => a.Id != currentWord.Id)
            .Select(a => a.KyrgyzWord)
            .Where(a => !string.Equals(a, currentWord.KyrgyzWord, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(_ => Guid.NewGuid())
            .ToList();

        if (wrongAnswersPool.Count < 3)
            throw new Exception("Недостаточно уникальных вариантов ответа для вопроса");

        return wrongAnswersPool
            .Take(3)
            .Append(currentWord.KyrgyzWord)
            .OrderBy(_ => Guid.NewGuid())
            .ToList();
    }
}
