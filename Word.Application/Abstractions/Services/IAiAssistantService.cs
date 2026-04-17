using Word.Application.DTOs.AI;


namespace Word.Application.Abstractions.Services;

public interface IAiAssistantService
{
    Task<AiAskResponseDto> AskAsync(AiAskRequestDto request, CancellationToken cancellationToken = default);
}
