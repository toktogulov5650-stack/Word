namespace Word.Infrastructure.Configurations;

public class GoogleAuthOptions
{
    public const string SectionName = "Authentication:Google";

    public string ClientId { get; set; } = string.Empty;
    public string[] ClientIds { get; set; } = [];

    public IReadOnlyCollection<string> GetAllowedClientIds()
    {
        return ClientIds
            .Append(ClientId)
            .Where(static clientId => !string.IsNullOrWhiteSpace(clientId))
            .Select(static clientId => clientId.Trim())
            .Distinct(StringComparer.Ordinal)
            .ToArray();
    }
}
