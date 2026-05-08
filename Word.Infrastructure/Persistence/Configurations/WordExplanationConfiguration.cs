using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Word.Domain.Entities;

namespace Word.Infrastructure.Persistence.Configurations;

public class WordExplanationConfiguration : IEntityTypeConfiguration<WordExplanation>
{
    public void Configure(EntityTypeBuilder<WordExplanation> builder)
    {
        builder.ToTable("WordExplanations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WordId)
            .IsRequired();

        builder.HasOne(x => x.Word)
            .WithOne(x => x.Explanation)
            .HasForeignKey<WordExplanation>(x => x.WordId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Translations)
            .WithOne(x => x.WordExplanation)
            .HasForeignKey(x => x.WordExplanationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Examples)
            .WithOne(x => x.WordExplanation)
            .HasForeignKey(x => x.WordExplanationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
