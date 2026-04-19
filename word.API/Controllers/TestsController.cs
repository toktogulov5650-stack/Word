using Microsoft.AspNetCore.Mvc;
using Word.Application.Abstractions.Services;
using Word.Application.DTOs.Tests;
using Word.API.Contracts.Tests;

namespace Word.API.Controllers;

[ApiController]
[Route("api/tests")]
public class TestsController : ControllerBase
{
    private readonly ITestService _testService;

    public TestsController(ITestService testService)
    {
        _testService = testService;
    }

    [HttpPost("start")]
    public async Task<ActionResult<StartTestResponse>> StartAsync(
        StartTestRequest request,
        CancellationToken cancellationToken = default)
    {
        var dto = new StartTestRequestDto
        {
            CategoryId = request.CategoryId
        };

        var result = await _testService.StartAsync(dto, cancellationToken);

        var response = new StartTestResponse
        {
            TestSessionId = result.TestSessionId,
            CurrentQuestion = new CurrentQuestionResponse
            {
                WordId = result.CurrentQuestion.WordId,
                EnglishWord = result.CurrentQuestion.EnglishWord,
                AnswerOptions = result.CurrentQuestion.AnswerOptions
            }
        };

        return Ok(response);
    }


    [HttpPost("answer")]
    public async Task<ActionResult<SubmitAnswerResponse>> SubmitAnswerAsync(SubmitAnswerRequest request, CancellationToken cancellationToken = default)
    {
        var dto = new SubmitAnswerRequestDto
        {
            TestSessionId = request.TestSessionId,
            WordId = request.WordId,
            SelectedAnswer = request.SelectedAnswer,
            IsMarkedUnknown = request.IsMarkedUnknown
        };


        var result = await _testService.SubmitAnswerAsync(dto, cancellationToken);

        var response = new SubmitAnswerResponse
        {
            IsCorrect = result.IsCorrect,
            CorrectAnswerCount = result.CorrectAnswerCount,
            IsFinished = result.IsFinished
        };


        if (result.CurrentQuestion is not null)
        {
            response.CurrentQuestion = new CurrentQuestionResponse
            {
                WordId = result.CurrentQuestion.WordId,
                EnglishWord = result.CurrentQuestion.EnglishWord,
                AnswerOptions = result.CurrentQuestion.AnswerOptions
            };
        }

        return Ok(response);
    }


    [HttpPost("{testSessionId:int}/finish")]
    public async Task<ActionResult<FinishTestResponse>> FinishAsync(int testSessionId, CancellationToken cancellationToken = default)
    {
        var result = await _testService.FinishAsync(testSessionId, cancellationToken);

        var response = new FinishTestResponse
        {
            CorrectAnswerCount = result.CorrectAnswerCount,
            BestScore = result.BestScore
        };

        return Ok(response);
    }
}
