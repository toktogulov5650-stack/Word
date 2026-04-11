using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Word.Domain.Entities;


namespace Word.Infrastructure.Persistence.Configurations;

public class CategoryRecordConfiguration : IEntityTypeConfiguration<CategoryRecord>
{
    public void Configure(EntityTypeBuilder<CategoryRecord> builder)
    {
        builder.ToTable("CategoryRecord");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.CategoryId)
            .IsRequired();

        builder.HasOne(a => a.Category)
            .WithMany()
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(a => a.BestScore)
            .IsRequired();
    }
}
