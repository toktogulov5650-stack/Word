using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
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

        ValidateConnectionString(connectionString);

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IWordRepository, WordRepository>();
        services.AddScoped<ITestSessionRepository, TestSessionRepository>();
        services.AddScoped<ICategoryRecordRepository, CategoryRecordRepository>();
        services.AddScoped<ITestQuestionRepository, TestQuestionRepository>();

        return services;
    }

    private static void ValidateConnectionString(string connectionString)
    {
        if (connectionString.Contains("YOUR_", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' still contains placeholder values. " +
                "Set a real PostgreSQL connection string in word.API/appsettings.Production.json.");
        }

        NpgsqlConnectionStringBuilder builder;

        try
        {
            builder = new NpgsqlConnectionStringBuilder(connectionString);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is invalid. " +
                "Check word.API/appsettings.Production.json and make sure the value is a single full PostgreSQL connection string.",
                ex);
        }

        if (string.IsNullOrWhiteSpace(builder.Host) ||
            string.IsNullOrWhiteSpace(builder.Database) ||
            string.IsNullOrWhiteSpace(builder.Username) ||
            string.IsNullOrWhiteSpace(builder.Password))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is incomplete. " +
                "Host, Database, Username, and Password are required in word.API/appsettings.Production.json.");
        }
    }
}
