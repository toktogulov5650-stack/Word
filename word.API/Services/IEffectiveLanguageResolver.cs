namespace Word.API.Services;

public interface IEffectiveLanguageResolver
{
    Task<string> ResolveAsync(string? requestedLanguageCode, CancellationToken cancellationToken = default);
}
