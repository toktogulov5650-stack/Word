using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
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
            .HasMaxLength(100);

        builder.Property(a => a.KyrgyzWord)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.CategoryId)
            .IsRequired();

        builder.HasOne(a => a.Category)
            .WithMany(a => a.Words)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
