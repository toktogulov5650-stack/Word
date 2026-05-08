using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Word.Domain.Constants;
using Word.Domain.Entities;

namespace Word.Infrastructure.Persistence.Configurations;

public class WordExplanationTranslationConfiguration : IEntityTypeConfiguration<WordExplanationTranslation>
{
    public void Configure(EntityTypeBuilder<WordExplanationTranslation> builder)
    {
        builder.ToTable("WordExplanationTranslations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WordExplanationId)
            .IsRequired();

        builder.Property(x => x.LanguageCode)
            .IsRequired()
            .HasMaxLength(DomainConstraints.LanguageCodeMaxLength);

        builder.Property(x => x.WhatIs)
            .IsRequired()
            .HasMaxLength(DomainConstraints.WordExplanationWhatIsMaxLength);

        builder.Property(x => x.Meaning)
            .IsRequired()
            .HasMaxLength(DomainConstraints.WordExplanationMeaningMaxLength);

        builder.Property(x => x.Translations)
            .IsRequired()
            .HasMaxLength(DomainConstraints.WordExplanationTranslationsMaxLength);

        builder.Property(x => x.Usage)
            .IsRequired()
            .HasMaxLength(DomainConstraints.WordExplanationUsageMaxLength);

        builder.Property(x => x.Hint)
            .IsRequired()
            .HasMaxLength(DomainConstraints.WordExplanationHintMaxLength);

        builder.HasIndex(x => new { x.WordExplanationId, x.LanguageCode })
            .IsUnique();
    }
}
