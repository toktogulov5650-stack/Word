using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Word.Domain.Constants;
using Word.Domain.Entities;

namespace Word.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.ImageUrl)
            .IsRequired()
            .HasMaxLength(DomainConstraints.CategoryImageUrlMaxLength)
            .HasDefaultValue(string.Empty);

        builder.HasMany(a => a.Translations)
            .WithOne(a => a.Category)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
