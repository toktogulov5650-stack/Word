using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Word.Application.Abstractions.Persistence;
using Word.Application.Abstractions.Services;
using Word.Infrastructure.Configurations;
using Word.Infrastructure.Persistence;
using Word.Infrastructure.Repositories;
using Word.Infrastructure.Services;

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

        services.Configure<GoogleAuthOptions>(
            configuration.GetSection(GoogleAuthOptions.SectionName));

        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.SectionName));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IWordRepository, WordRepository>();
        services.AddScoped<ITestSessionRepository, TestSessionRepository>();
        services.AddScoped<ICategoryRecordRepository, CategoryRecordRepository>();
        services.AddScoped<ITestQuestionRepository, TestQuestionRepository>();
        services.AddScoped<IWordExplanationRepository, WordExplanationsRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IGoogleTokenVerifier, GoogleTokenVerifier>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();

        return services;
    }
}
