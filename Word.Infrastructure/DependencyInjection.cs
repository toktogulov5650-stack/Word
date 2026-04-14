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
        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? configuration["ConnectionStrings__DefaultConnection"]
            ?? configuration["DEFAULT_CONNECTION"];

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "Connection string was not found. Checked: ConnectionStrings:DefaultConnection, ConnectionStrings__DefaultConnection, DEFAULT_CONNECTION.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IWordRepository, WordRepository>();
        services.AddScoped<ITestSessionRepository, TestSessionRepository>();
        services.AddScoped<ICategoryRecordRepository, CategoryRecordRepository>();
        services.AddScoped<ITestQuestionRepository, TestQuestionRepository>();

        return services;
    }
}
