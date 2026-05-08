using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Word.Domain.Constants;
using Word.Domain.Entities;

namespace Word.Infrastructure.Persistence.Configurations;

public class WordExampleTranslationConfiguration : IEntityTypeConfiguration<WordExampleTranslation>
{
    public void Configure(EntityTypeBuilder<WordExampleTranslation> builder)
    {
        builder.ToTable("WordExampleTranslations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WordExampleId)
            .IsRequired();

        builder.Property(x => x.LanguageCode)
            .IsRequired()
            .HasMaxLength(DomainConstraints.LanguageCodeMaxLength);

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(DomainConstraints.WordExplanationExampleMaxLength);

        builder.Property(x => x.Translation)
            .IsRequired()
            .HasMaxLength(DomainConstraints.WordExplanationExampleMaxLength);

        builder.HasIndex(x => new { x.WordExampleId, x.LanguageCode })
            .IsUnique();
    }
}
