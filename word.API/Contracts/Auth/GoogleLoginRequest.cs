using System.ComponentModel.DataAnnotations;

namespace Word.API.Contracts.Auth;

public class GoogleLoginRequest
{
    [Required]
    public string IdToken { get; set; } = string.Empty;
}
