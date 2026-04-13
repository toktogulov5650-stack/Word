using Word.Application;
using Word.Infrastructure;
using Word.Infrastructure.Persistence;
using Word.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>()?
    .Where(origin => !string.IsNullOrWhiteSpace(origin))
    .ToList()
    ?? [];

var allowedOriginsCsv = builder.Configuration["Cors:AllowedOriginsCsv"]
    ?? Environment.GetEnvironmentVariable("CORS_ALLOWED_ORIGINS");

if (!string.IsNullOrWhiteSpace(allowedOriginsCsv))
{
    allowedOrigins.AddRange(
        allowedOriginsCsv
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
}

var distinctAllowedOrigins = allowedOrigins
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .ToArray();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        if (distinctAllowedOrigins.Length > 0)
        {
            policy
                .WithOrigins(distinctAllowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen();


var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbInitializer.InitializeAsync(context);
}

if (builder.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseCors("Frontend");

app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();
app.Run();
