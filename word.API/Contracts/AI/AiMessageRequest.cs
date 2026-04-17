using System.ComponentModel.DataAnnotations;


namespace Word.API.Contracts.AI;

public class AiMessageRequest
{
    [Required]
    [MaxLength(20)]
    public string Role { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;
}
