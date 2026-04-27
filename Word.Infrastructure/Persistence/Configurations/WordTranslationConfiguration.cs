using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

        builder.Property(a => a.KyrgyzWord)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(a => a.Word)
            .WithMany(a => a.WordTranslations)
            .HasForeignKey(a => a.WordId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
