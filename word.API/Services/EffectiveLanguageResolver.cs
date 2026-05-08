using System.Security.Claims;
using Word.Application.Abstractions.Services;
using Word.Application.Localization;

namespace Word.API.Services;

public class EffectiveLanguageResolver : IEffectiveLanguageResolver
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthService _authService;

    public EffectiveLanguageResolver(
        IHttpContextAccessor httpContextAccessor,
        IAuthService authService)
    {
        _httpContextAccessor = httpContextAccessor;
        _authService = authService;
    }

    public async Task<string> ResolveAsync(
        string? requestedLanguageCode,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(requestedLanguageCode))
            return LocalizedContentResolver.NormalizeRequestedLanguage(requestedLanguageCode);

        var userIdValue = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out var userId))
            return LocalizedContentResolver.DefaultLanguageCode;

        var user = await _authService.GetCurrentUserAsync(userId, cancellationToken);

        return user is null
            ? LocalizedContentResolver.DefaultLanguageCode
            : LocalizedContentResolver.NormalizeRequestedLanguage(user.PreferredLanguage);
    }
}
