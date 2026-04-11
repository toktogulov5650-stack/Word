using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Word.Infrastructure.Configuration;

internal static class ConnectionStringResolver
{
    public static string Resolve(IConfiguration configuration)
    {
        var configuredConnectionString = configuration.GetConnectionString("DefaultConnection");

        if (!string.IsNullOrWhiteSpace(configuredConnectionString))
        {
            return configuredConnectionString;
        }

        var databaseUrl = configuration["DATABASE_URL"];

        if (!string.IsNullOrWhiteSpace(databaseUrl))
        {
            return ConvertDatabaseUrlToConnectionString(databaseUrl);
        }

        throw new InvalidOperationException(
            "Database connection string was not found. Configure 'ConnectionStrings__DefaultConnection' or 'DATABASE_URL'.");
    }

    private static string ConvertDatabaseUrlToConnectionString(string databaseUrl)
    {
        if (!Uri.TryCreate(databaseUrl, UriKind.Absolute, out var databaseUri))
        {
            throw new InvalidOperationException("The 'DATABASE_URL' value is not a valid absolute URI.");
        }

        var userInfo = databaseUri.UserInfo.Split(':', 2);
        var username = Uri.UnescapeDataString(userInfo.ElementAtOrDefault(0) ?? string.Empty);
        var password = Uri.UnescapeDataString(userInfo.ElementAtOrDefault(1) ?? string.Empty);
        var databaseName = databaseUri.AbsolutePath.Trim('/');

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,
            Port = databaseUri.IsDefaultPort ? 5432 : databaseUri.Port,
            Username = username,
            Password = password,
            Database = databaseName
        };

        var queryParameters = ParseQueryParameters(databaseUri.Query);

        if (queryParameters.TryGetValue("sslmode", out var sslMode) &&
            !string.IsNullOrWhiteSpace(sslMode))
        {
            builder.SslMode = Enum.Parse<SslMode>(sslMode, ignoreCase: true);
        }

        if (queryParameters.TryGetValue("trust_server_certificate", out var trustServerCertificate) &&
            !string.IsNullOrWhiteSpace(trustServerCertificate))
        {
            builder.TrustServerCertificate = bool.Parse(trustServerCertificate);
        }

        return builder.ConnectionString;
    }

    private static Dictionary<string, string> ParseQueryParameters(string queryString)
    {
        var parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var pair in queryString.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = pair.Split('=', 2);
            var key = Uri.UnescapeDataString(parts[0]);
            var value = parts.Length > 1 ? Uri.UnescapeDataString(parts[1]) : string.Empty;

            parameters[key] = value;
        }

        return parameters;
    }
}
