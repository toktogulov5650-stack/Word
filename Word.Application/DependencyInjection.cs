using Microsoft.Extensions.DependencyInjection;
using Word.Application.Abstractions.Services;
using Word.Application.Features.Categories;
using Word.Application.Features.Records;
using Word.Application.Features.Tests;

namespace Word.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IRecordService, RecordService>();
        services.AddScoped<ITestService, TestService>();

        return services;
    }
}
