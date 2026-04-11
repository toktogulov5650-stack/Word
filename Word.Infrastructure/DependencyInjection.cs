using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Word.Application.Abstractions.Persistence;
using Word.Infrastructure.Configuration;
using Word.Infrastructure.Persistence;
using Word.Infrastructure.Repositories;


namespace Word.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = ConnectionStringResolver.Resolve(configuration);

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

