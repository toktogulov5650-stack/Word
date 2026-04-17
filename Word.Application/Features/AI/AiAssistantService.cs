using Word.Application.Abstractions.Persistence;
using Word.Application.Abstractions.Services;
using Word.Application.DTOs.AI;

namespace Word.Application.Features.AI;

public class AiAssistantService : IAiAssistantService
{
    private readonly IWordRepository _wordRepository;
    private readonly IAiTextGenerationClient _aiTextGenerationClient;

    public AiAssistantService(
        IWordRepository wordRepository,
        IAiTextGenerationClient aiTextGenerationClient)
    {
        _wordRepository = wordRepository;
        _aiTextGenerationClient = aiTextGenerationClient;
    }

    public async Task<AiAskResponseDto> AskAsync(
        AiAskRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var words = new List<string>();

        if (request.WordIds.Count > 0)
        {
            var databaseWords = await _wordRepository.GetByIdsAsync(request.WordIds, cancellationToken);

            words.AddRange(
                databaseWords
                    .Select(a => a.EnglishWord)
                    .Where(a => !string.IsNullOrWhiteSpace(a)));
        }

        if (request.CustomWords.Count > 0)
        {
            words.AddRange(
                request.CustomWords
                    .Where(a => !string.IsNullOrWhiteSpace(a))
                    .Select(a => a.Trim()));
        }

        words = words
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (words.Count == 0)
            throw new Exception("Для запроса к ИИ нужно передать хотя бы одно слово");

        var answer = await _aiTextGenerationClient.GenerateAnswerAsync(
            words,
            request.Messages,
            cancellationToken);

        return new AiAskResponseDto
        {
            Answer = answer
        };
    }
}
