using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Word.Application.Abstractions.Persistence;
using Word.Infrastructure.Persistence;
using Word.Infrastructure.Repositories;

namespace Word.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IWordRepository, WordRepository>();
        services.AddScoped<ITestSessionRepository, TestSessionRepository>();
        services.AddScoped<ICategoryRecordRepository, CategoryRecordRepository>();
        services.AddScoped<ITestQuestionRepository, TestQuestionRepository>();
        services.AddScoped<IWordExplanationRepository, WordExplanationsRepository>();

        return services;
    }
}
