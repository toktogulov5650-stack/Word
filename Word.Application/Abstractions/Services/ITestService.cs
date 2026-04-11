

using Word.Application.DTOs.Tests;

namespace Word.Application.Abstractions.Services;

public interface ITestService
{
    Task<StartTestResponseDto> StartAsync(StartTestRequestDto request, CancellationToken cancellationToken = default);
    Task<SubmitAnswerResponseDto> SubmitAnswerAsync(SubmitAnswerRequestDto request, CancellationToken cancellationToken = default);
    Task<FinishTestResponseDto> FinishAsync(int testSessionId, CancellationToken cancellationToken = default);
}
