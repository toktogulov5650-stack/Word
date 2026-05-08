using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Word.Domain.Constants;
using Word.Domain.Entities;

namespace Word.Infrastructure.Persistence.Configurations;

public class CategoryTranslationConfiguration : IEntityTypeConfiguration<CategoryTranslation>
{
    public void Configure(EntityTypeBuilder<CategoryTranslation> builder)
    {
        builder.ToTable("CategoryTranslations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CategoryId)
            .IsRequired();

        builder.Property(x => x.LanguageCode)
            .IsRequired()
            .HasMaxLength(DomainConstraints.LanguageCodeMaxLength);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(DomainConstraints.CategoryNameMaxLength);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(DomainConstraints.CategoryDescriptionMaxLength);

        builder.HasIndex(x => new { x.CategoryId, x.LanguageCode })
            .IsUnique();
    }
}
