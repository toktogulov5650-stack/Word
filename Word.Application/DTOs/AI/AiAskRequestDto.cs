

namespace Word.Application.DTOs.AI;

public class AiAskRequestDto
{
    public List<int> WordIds { get; set; } = [];

    public List<string> CustomWords { get; set; } = [];

    public List<AiMessageDto> Messages { get; set; } = [];

}
