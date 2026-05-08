using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Word.Domain.Entities;

namespace Word.Infrastructure.Persistence.Configurations;

public class WordExampleConfiguration : IEntityTypeConfiguration<WordExample>
{
    public void Configure(EntityTypeBuilder<WordExample> builder)
    {
        builder.ToTable("WordExamples");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WordExplanationId)
            .IsRequired();

        builder.Property(x => x.SortOrder)
            .IsRequired();

        builder.HasIndex(x => new { x.WordExplanationId, x.SortOrder })
            .IsUnique();

        builder.HasMany(x => x.Translations)
            .WithOne(x => x.WordExample)
            .HasForeignKey(x => x.WordExampleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
