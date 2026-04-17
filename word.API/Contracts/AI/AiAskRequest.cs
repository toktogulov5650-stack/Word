using System.ComponentModel.DataAnnotations;


namespace Word.API.Contracts.AI;

public class AiAskRequest
{
    public List<int> WordIds { get; set; } = [];

    public List<string> CustomWords { get; set; } = [];

    public List<AiMessageRequest> Messages { get; set; } = [];

}
