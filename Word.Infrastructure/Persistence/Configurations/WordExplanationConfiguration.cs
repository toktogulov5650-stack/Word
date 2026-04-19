using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Word.Domain.Entities;

namespace Word.Infrastructure.Persistence.Configurations;

public class WordExplanationConfiguration : IEntityTypeConfiguration<WordExplanation>
{
    public void Configure(EntityTypeBuilder<WordExplanation> builder)
    {
        builder.ToTable("WordExplanations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WordId)
            .IsRequired();

        builder.Property(x => x.WhatIs)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Meaning)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.Translations)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.Usage)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.Example1)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Example2)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Example3)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Hint)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasOne(x => x.Word)
            .WithOne(x => x.Explanation)
            .HasForeignKey<WordExplanation>(x => x.WordId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
