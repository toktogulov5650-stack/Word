using Word.Domain.Constants;


namespace Word.Domain.Entities;

public class Category
{
    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    private readonly List<WordEntity> _words = new();
    public IReadOnlyCollection<WordEntity> Words => _words;

    public Category(string name, string description)
    {

        if (name.Length > DomainConstraints.CategoryNameMaxLength)
        {
            throw new Exception("Category name is too long");
        }

        if (description.Length > DomainConstraints.CategoryDescriptionMaxLength)
        {
            throw new Exception("Category description is too long");
        }

        Name = name;
        Description = description;
        IsActive = true;
    }
}
