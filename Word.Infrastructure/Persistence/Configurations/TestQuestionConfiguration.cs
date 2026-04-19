using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Word.Domain.Entities;

namespace Word.Infrastructure.Persistence.Configurations;

public class TestQuestionConfiguration : IEntityTypeConfiguration<TestQuestion>
{
    public void Configure(EntityTypeBuilder<TestQuestion> builder)
    {
        builder.ToTable("TestQuestions");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.TestSessionId)
            .IsRequired();

        builder.HasOne(a => a.TestSession)
            .WithMany(a => a.TestQuestions)
            .HasForeignKey(a => a.TestSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Word)
            .WithMany()
            .HasForeignKey(a => a.WordId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(a => a.WordId)
            .IsRequired();

        builder.Property(a => a.QuestionOrder)
            .IsRequired();

        builder.Property(a => a.IsAnswered)
            .IsRequired();

        builder.Property(a => a.IsCorrect)
            .IsRequired();

        builder.Property(a => a.IsMarkedUnknown)
            .IsRequired();
    }
}
