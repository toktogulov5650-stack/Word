using Word.Application.DTOs.AI;


namespace Word.Application.Abstractions.Services;

public interface IAiTextGenerationClient
{
    Task<string> GenerateAnswerAsync(
        IReadOnlyCollection<string> words,
        IReadOnlyCollection<AiMessageDto> messages,
        CancellationToken cancellationToken = default);
}
