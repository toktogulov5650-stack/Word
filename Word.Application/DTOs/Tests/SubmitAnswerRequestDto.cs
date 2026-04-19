

using System.ComponentModel.DataAnnotations;

namespace Word.Application.DTOs.Tests;

public class SubmitAnswerRequestDto
{
    [Range(1, int.MaxValue)]
    public int TestSessionId { get; set; }

    public int WordId { get; set; }


    [MaxLength(100)]
    public string SelectedAnswer { get; set; } = string.Empty;

    public bool IsMarkedUnknown { get; set; }
}
