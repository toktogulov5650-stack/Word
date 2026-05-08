using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Word.Domain.Constants;
using Word.Domain.Entities;

namespace Word.Infrastructure.Persistence.Configurations;

public class WordTranslationConfiguration : IEntityTypeConfiguration<WordTranslation>
{
    public void Configure(EntityTypeBuilder<WordTranslation> builder)
    {
        builder.ToTable("WordTranslations");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.WordId)
            .IsRequired();

        builder.Property(a => a.LanguageCode)
            .IsRequired()
            .HasMaxLength(DomainConstraints.LanguageCodeMaxLength)
            .HasDefaultValue("ky");

        builder.Property(a => a.Text)
            .IsRequired()
            .HasMaxLength(DomainConstraints.WordTranslationTextMaxLength);

        builder.HasIndex(a => new { a.WordId, a.LanguageCode, a.Text })
            .IsUnique();

        builder.HasOne(a => a.Word)
            .WithMany(a => a.WordTranslations)
            .HasForeignKey(a => a.WordId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
