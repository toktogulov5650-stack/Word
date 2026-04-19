using Microsoft.Extensions.DependencyInjection;
using Word.Application.Abstractions.Services;
using Word.Application.Features.Categories;
using Word.Application.Features.Records;
using Word.Application.Features.Tests;
using Word.Application.Features.WordExplanations;

namespace Word.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IRecordService, RecordService>();
        services.AddScoped<ITestService, TestService>();
        services.AddScoped<IWordExplanationService, WordExplanationService>();

        return services;
    }
}
