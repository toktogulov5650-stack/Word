

namespace Word.API.Contracts.Records;

public class CategoryRecordResponse
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int BestScore { get; set; }
}

