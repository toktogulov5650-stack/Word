using System.ComponentModel.DataAnnotations;

namespace Word.Application.DTOs.Tests;

public class StartTestRequestDto
{
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    [MaxLength(10)]
    public string LanguageCode { get; set; } = string.Empty;
}
