using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Word.Domain.Entities;


namespace Word.Infrastructure.Persistence.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.GoogleId)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.CreatedAtUtc)
            .IsRequired();

        builder.Property(a => a.LastLoginAtUtc)
            .IsRequired();

        builder.HasIndex(a => a.GoogleId)
            .IsUnique();
    }
}
