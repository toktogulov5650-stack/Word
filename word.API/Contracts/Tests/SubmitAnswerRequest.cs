using System.ComponentModel.DataAnnotations;


namespace Word.API.Contracts.Tests;

public class SubmitAnswerRequest
{
    [Range(1, int.MaxValue)]
    public int TestSessionId { get; set; }

    [Range(1, int.MaxValue)]
    public int WordId { get; set; }

    [Required]
    [MaxLength(100)]
    public string SelectedAnswer { get; set; } = string.Empty;
}
