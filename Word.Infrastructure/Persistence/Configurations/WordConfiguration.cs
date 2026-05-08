using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Word.Domain.Constants;
using Word.Domain.Entities;

namespace Word.Infrastructure.Persistence.Configurations;

public class WordConfiguration : IEntityTypeConfiguration<WordEntity>
{
    public void Configure(EntityTypeBuilder<WordEntity> builder)
    {
        builder.ToTable("Words");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.EnglishWord)
            .IsRequired()
            .HasMaxLength(DomainConstraints.EnglishWordMaxLength);

        builder.Property(a => a.CategoryId)
            .IsRequired();

        builder.HasOne(a => a.Category)
            .WithMany(a => a.Words)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.WordTranslations)
            .WithOne(a => a.Word)
            .HasForeignKey(a => a.WordId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Explanation)
            .WithOne(a => a.Word)
            .HasForeignKey<WordExplanation>(a => a.WordId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
