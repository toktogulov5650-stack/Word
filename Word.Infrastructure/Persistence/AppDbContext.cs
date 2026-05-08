using Microsoft.EntityFrameworkCore;
using Word.Domain.Entities;


namespace Word.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<CategoryTranslation> CategoryTranslations => Set<CategoryTranslation>();
    public DbSet<CategoryRecord> CategoryRecords => Set<CategoryRecord>();
    public DbSet<TestSession> TestSessions => Set<TestSession>();
    public DbSet<WordEntity> WordEntities => Set<WordEntity>();
    public DbSet<TestQuestion> TestQuestions => Set<TestQuestion>();
    public DbSet<WordExplanation> WordExplanations => Set<WordExplanation>();
    public DbSet<WordExplanationTranslation> WordExplanationTranslations => Set<WordExplanationTranslation>();
    public DbSet<WordExample> WordExamples => Set<WordExample>();
    public DbSet<WordExampleTranslation> WordExampleTranslations => Set<WordExampleTranslation>();
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<WordTranslation> WordTranslations => Set<WordTranslation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
