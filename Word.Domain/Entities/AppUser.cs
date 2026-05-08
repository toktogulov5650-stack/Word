using Word.Domain.Constants;

namespace Word.Domain.Entities;

public class AppUser
{
    private AppUser()
    {
    }

    public AppUser(
        string name,
        string email,
        string? googleId,
        string? passwordHash,
        string preferredLanguage = LanguageCodes.Default)
    {
        Name = NormalizeRequired(name, nameof(name));
        Email = NormalizeEmail(email);
        GoogleId = googleId;
        PasswordHash = passwordHash;
        PreferredLanguage = LanguageCodes.NormalizeOrDefault(preferredLanguage, nameof(preferredLanguage));
        CreatedAtUtc = DateTime.UtcNow;
        LastLoginAtUtc = DateTime.UtcNow;
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? GoogleId { get; private set; }
    public string? PasswordHash { get; private set; }
    public string PreferredLanguage { get; private set; } = LanguageCodes.Default;
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime LastLoginAtUtc { get; private set; }

    public void UpdateProfile(string name, string email)
    {
        Name = NormalizeRequired(name, nameof(name));
        Email = NormalizeEmail(email);
        LastLoginAtUtc = DateTime.UtcNow;
    }

    public void SetGoogleId(string googleId)
    {
        if (string.IsNullOrWhiteSpace(googleId))
            throw new ArgumentException("GoogleId is required.", nameof(googleId));

        GoogleId = googleId.Trim();
    }

    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));

        PasswordHash = passwordHash;
    }

    public void MarkLogin()
    {
        LastLoginAtUtc = DateTime.UtcNow;
    }

    public void SetPreferredLanguage(string languageCode)
    {
        PreferredLanguage = LanguageCodes.NormalizeSupported(languageCode, nameof(languageCode));
    }

    private static string NormalizeRequired(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} is required.", paramName);

        return value.Trim();
    }

    private static string NormalizeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        return email.Trim().ToLowerInvariant();
    }
}
