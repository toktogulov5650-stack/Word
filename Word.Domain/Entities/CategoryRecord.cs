namespace Word.Domain.Entities;

public class CategoryRecord
{
    private CategoryRecord()
    {
    }

    public CategoryRecord(int categoryId, int bestScore, int bestTotalQuestionCount)
    {
        if (categoryId <= 0)
            throw new ArgumentOutOfRangeException(nameof(categoryId), "Category ID must be greater than zero.");

        if (bestScore < 0)
            throw new ArgumentOutOfRangeException(nameof(bestScore), "Best score cannot be negative.");

        if (bestTotalQuestionCount < 0)
            throw new ArgumentOutOfRangeException(nameof(bestTotalQuestionCount), "Best total question count cannot be negative.");

        CategoryId = categoryId;
        BestScore = bestScore;
        BestTotalQuestionCount = bestTotalQuestionCount;
    }

    public int Id { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public int BestScore { get; private set; }
    public int BestTotalQuestionCount { get; private set; }

    public void UpdateBestScore(int bestScore, int bestTotalQuestionCount)
    {
        if (bestScore < 0)
            throw new ArgumentOutOfRangeException(nameof(bestScore), "Best score cannot be negative.");

        if (bestTotalQuestionCount < 0)
            throw new ArgumentOutOfRangeException(nameof(bestTotalQuestionCount), "Best total question count cannot be negative.");

        BestScore = bestScore;
        BestTotalQuestionCount = bestTotalQuestionCount;
    }
}
