using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Word.Domain.Constants;
using Word.Domain.Entities;

namespace Word.Infrastructure.Persistence.Configurations;

public class TestSessionConfiguration : IEntityTypeConfiguration<TestSession>
{
    public void Configure(EntityTypeBuilder<TestSession> builder)
    {
        builder.ToTable("TestSessions");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.CategoryId)
            .IsRequired();

        builder.HasOne(a => a.Category)
            .WithMany()
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(a => a.LanguageCode)
            .IsRequired()
            .HasMaxLength(DomainConstraints.LanguageCodeMaxLength)
            .HasDefaultValue("ru");

        builder.Property(a => a.Status)
            .IsRequired();

        builder.Property(a => a.CorrectAnswerCount)
            .IsRequired();

        builder.Property(a => a.TotalQuestionCount)
            .IsRequired();

        builder.HasMany(a => a.TestQuestions)
            .WithOne(a => a.TestSession)
            .HasForeignKey(a => a.TestSessionId);
    }
}
