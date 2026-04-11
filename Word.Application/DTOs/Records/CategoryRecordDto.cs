

using System.ComponentModel.DataAnnotations;

namespace Word.Application.DTOs.Records;

public class CategoryRecordDto
{
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public int BestScore { get; set; }
}
