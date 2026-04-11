namespace Word.Domain.Entities;

public class CategoryRecord
{
    private CategoryRecord()
    {
    }

    public CategoryRecord(int categoryId, int bestScore)
    {
        CategoryId = categoryId;
        BestScore = bestScore;
    }

    public int Id { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public int BestScore { get; private set; }

    public void UpdateBestScore(int bestScore)
    {
        BestScore = bestScore;
    }
}
