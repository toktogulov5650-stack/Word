using System.ComponentModel.DataAnnotations;

namespace Word.API.Contracts.Auth;

public class GoogleLoginRequest
{
    [Required]
    public string IdToken { get; set; } = string.Empty;

    [StringLength(2, MinimumLength = 2)]
    public string? PreferredLanguage { get; set; }
}
