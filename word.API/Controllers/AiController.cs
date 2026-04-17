using Microsoft.AspNetCore.Mvc;
using Word.Application.Abstractions.Services;
using Word.Application.DTOs.AI;
using Word.API.Contracts.AI;

namespace Word.API.Controllers;

[ApiController]
[Route("api/ai")]
public class AiController : ControllerBase
{
    private readonly IAiAssistantService _aiAssistantService;

    public AiController(IAiAssistantService aiAssistantService)
    {
        _aiAssistantService = aiAssistantService;
    }

    [HttpPost("ask")]
    public async Task<ActionResult<AiAskResponse>> AskAsync(
        AiAskRequest request,
        CancellationToken cancellationToken = default)
    {
        var dto = new AiAskRequestDto
        {
            WordIds = request.WordIds,
            CustomWords = request.CustomWords,
            Messages = request.Messages
                .Select(x => new AiMessageDto
                {
                    Role = x.Role,
                    Content = x.Content
                })
                .ToList()
        };

        var result = await _aiAssistantService.AskAsync(dto, cancellationToken);

        var response = new AiAskResponse
        {
            Answer = result.Answer
        };

        return Ok(response);
    }
}
