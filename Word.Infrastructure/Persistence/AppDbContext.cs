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
    public DbSet<CategoryRecord> CategoryRecords => Set<CategoryRecord>();
    public DbSet<TestSession> TestSessions => Set<TestSession>();
    public DbSet<WordEntity> WordEntities => Set<WordEntity>();
    public DbSet<TestQuestion> TestQuestions => Set<TestQuestion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
